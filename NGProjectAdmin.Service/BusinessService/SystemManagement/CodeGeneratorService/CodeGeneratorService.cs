using Masuit.Tools.Strings;
using Newtonsoft.Json;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.CoreDTO;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.CodeGeneratorRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.CodeGeneratorService
{
    /// <summary>
    /// 代码生产服务层实现
    /// </summary>
    public class CodeGeneratorService : ICodeGeneratorService
    {
        #region 属性及其构造函数

        /// <summary>
        /// 代码生产仓储层实例
        /// </summary>
        private readonly ICodeGeneratorRepository CodeGeneratorRepository;

        /// <summary>
        /// 无参构造函数
        /// </summary>
        /// <param name="CodeGeneratorRepository"></param>
        public CodeGeneratorService(ICodeGeneratorRepository CodeGeneratorRepository)
        {
            this.CodeGeneratorRepository = CodeGeneratorRepository;
        }

        #endregion

        #region 获取表名称列表

        /// <summary>
        /// 获取表名称列表
        /// </summary>
        /// <returns>表名称列表</returns>
        public async Task<List<DbSchemaInfoDTO>> GetSchemaInfo()
        {
            return await this.CodeGeneratorRepository.GetSchemaInfo();
        }

        #endregion

        #region 自动生成代码

        /// <summary>
        /// 自动生成代码
        /// </summary>
        /// <param name="codeGenerator">参数</param>
        /// <returns>zipId</returns>
        public async Task<Guid> CodeGenerate(CodeGeneratorDTO codeGenerator)
        {
            var zipId = Guid.NewGuid();
            var tempPath = NGAdminGlobalContext.DirectoryConfig.GetTempPath() + "/" + zipId;
            var entityList = new List<DbSchemaEntity>();

            #region 获取元数据信息

            var fields = await this.CodeGeneratorRepository.GetSchemaFieldsInfo(codeGenerator.Tables);
            foreach (var item in fields)
            {
                if (entityList.Count <= 0 || entityList.Where(t => t.EntityName == item.TableName).ToList().Count == 0)
                {
                    var entity = new DbSchemaEntity() { EntityName = item.TableName };
                    entity.Fields.Add(new DbSchemaField()
                    {
                        FieldName = item.FieldName.ToUpper(),
                        FieldDataType = item.GetDataType(),
                        FieldComment = item.FieldComment,
                        IsNullable = item.IsNullable,
                        FieldMaxLength = item.FieldMaxLength
                    });

                    entityList.Add(entity);
                }
                else
                {
                    var entity = entityList.Where(t => t.EntityName == item.TableName).FirstOrDefault();
                    entity.Fields.Add(new DbSchemaField()
                    {
                        FieldName = item.FieldName.ToUpper(),
                        FieldDataType = item.GetDataType(),
                        FieldComment = item.FieldComment,
                        IsNullable = item.IsNullable,
                        FieldMaxLength = item.FieldMaxLength
                    });
                }
            }

            #endregion

            //补充全路径
            codeGenerator.AutoFillFullName();

            #region 生成业务文件夹

            //生成实体模型文件夹
            var entityPath = Path.Join(tempPath, "/", "Entity");
            NGFileContext.CreateDirectory(entityPath);

            //生成DTO模型文件夹
            var dtoPath = Path.Join(tempPath, "/", "DTO");
            NGFileContext.CreateDirectory(dtoPath);

            //生成控制层文件夹
            var controllerPath = Path.Join(tempPath, "/", "Controller");
            NGFileContext.CreateDirectory(controllerPath);

            //生成服务层文件夹
            var servicePath = Path.Join(tempPath, "/", "Service");
            NGFileContext.CreateDirectory(servicePath);

            //生成仓储层文件夹
            var repositoryPath = Path.Join(tempPath, "/", "Repository");
            NGFileContext.CreateDirectory(repositoryPath);

            //生成视图层文件夹
            var viewPath = Path.Join(tempPath, "/", "Vue");
            NGFileContext.CreateDirectory(viewPath);

            #endregion

            foreach (var item in entityList)
            {
                #region 处理表名称

                if (item.EntityName.Contains("_"))
                {
                    var strs = item.EntityName.Split('_');
                    item.EntityName = String.Empty;
                    foreach (var name in strs)
                    {
                        item.EntityName += item.ToTitleCase(name);
                    }
                }

                #endregion

                #region 生成实体模型

                var sqlEntityKey = "sqls:sql:template_entity";
                var strEntitySQL = NGAdminGlobalContext.Configuration.GetSection(sqlEntityKey).Value;

                var entityTemplate = new Template(strEntitySQL);

                entityTemplate.Set("EntityName", item.EntityName);
                entityTemplate.Set("Copyright", DateTime.Now.Year + " RuYiAdmin All Rights Reserved");
                entityTemplate.Set("Version", Environment.Version.ToString());
                entityTemplate.Set("Author", "auto generated by RuYiAdmin Template Code Generator");
                entityTemplate.Set("DateTime", DateTime.Now.ToString());
                entityTemplate.Set("EntityNamespace", codeGenerator.EntityNamespace);

                #region 处理字段属性

                var properties = new StringBuilder();
                foreach (var field in item.Fields)
                {
                    if (!field.IsFieldIgnoreCase())
                    {
                        //处理字段注释
                        properties.AppendLine("		/// <summary>");
                        properties.AppendLine($"		/// {field.FieldComment}");
                        properties.AppendLine("		/// </summary>");

                        //处理必填项
                        if (field.IsNullable.ToUpper() == "NO")
                        {
                            properties.AppendLine("		[Required]");
                        }

                        //处理最大长度
                        if (!String.IsNullOrEmpty(field.FieldMaxLength) && !field.FieldDataType.Equals("Guid"))
                        {
                            properties.AppendLine($"		[MaxLength({field.FieldMaxLength})]");
                        }

                        //定义属性
                        if (field.IsNullable.ToUpper() == "NO")
                        {
                            properties.AppendLine($"		public {field.FieldDataType} {field.FieldName}" + " { get; set; }");
                        }
                        else
                        {
                            if (!field.FieldDataType.Equals("String"))
                            {
                                properties.AppendLine($"		public Nullable<{field.FieldDataType}> {field.FieldName}" + " { get; set; }");
                            }
                            else
                            {
                                properties.AppendLine($"		public {field.FieldDataType} {field.FieldName}" + " { get; set; }");
                            }
                        }
                        properties.AppendLine(String.Empty);
                    }
                }

                #endregion

                entityTemplate.Set("Fields", properties.ToString().TrimEnd('\r').TrimEnd('\n'));

                String entityContent = entityTemplate.Render();

                var entityDocument = Path.Join(entityPath, "/", $"{item.EntityName}.cs");
                NGFileContext.CreateFile(entityDocument);
                NGFileContext.WriteText(entityDocument, entityContent);

                #endregion

                #region 生成DTO模型

                var sqlDtoKey = "sqls:sql:template_dto";
                var strDtoSQL = NGAdminGlobalContext.Configuration.GetSection(sqlDtoKey).Value;

                var dtoTemplate = new Template(strDtoSQL);

                dtoTemplate.Set("EntityName", item.EntityName);
                dtoTemplate.Set("Copyright", DateTime.Now.Year + " RuYiAdmin All Rights Reserved");
                dtoTemplate.Set("Version", Environment.Version.ToString());
                dtoTemplate.Set("Author", "auto generated by RuYiAdmin Template Code Generator");
                dtoTemplate.Set("DateTime", DateTime.Now.ToString());
                dtoTemplate.Set("DTONamespace", codeGenerator.DTONamespace);

                String dtoContent = dtoTemplate.Render();

                var dtoDocument = Path.Join(dtoPath, "/", $"{item.EntityName}DTO.cs");
                NGFileContext.CreateFile(dtoDocument);
                NGFileContext.WriteText(dtoDocument, dtoContent);

                #endregion

                #region 生成控制层

                var sqlControllerKey = "sqls:sql:template_controller";
                var strControllerSQL = NGAdminGlobalContext.Configuration.GetSection(sqlControllerKey).Value;

                var controllerTemplate = new Template(strControllerSQL);

                controllerTemplate.Set("EntityName", item.EntityName);
                controllerTemplate.Set("Copyright", DateTime.Now.Year + " RuYiAdmin All Rights Reserved");
                controllerTemplate.Set("Version", Environment.Version.ToString());
                controllerTemplate.Set("Author", "auto generated by RuYiAdmin Template Code Generator");
                controllerTemplate.Set("DateTime", DateTime.Now.ToString());
                controllerTemplate.Set("ControllerNamespace", codeGenerator.ControllerNamespace);

                String controllerContent = controllerTemplate.Render();

                var controllerDocument = Path.Join(controllerPath, "/", $"{item.EntityName}ManagementController.cs");
                NGFileContext.CreateFile(controllerDocument);
                NGFileContext.WriteText(controllerDocument, controllerContent);

                #endregion

                #region 生成服务层

                #region 生成服务层接口

                var sqlIServiceKey = "sqls:sql:template_iservice";
                var strIServiceSQL = NGAdminGlobalContext.Configuration.GetSection(sqlIServiceKey).Value;

                var iServiceTemplate = new Template(strIServiceSQL);

                iServiceTemplate.Set("EntityName", item.EntityName);
                iServiceTemplate.Set("Copyright", DateTime.Now.Year + " RuYiAdmin All Rights Reserved");
                iServiceTemplate.Set("Version", Environment.Version.ToString());
                iServiceTemplate.Set("Author", "auto generated by RuYiAdmin Template Code Generator");
                iServiceTemplate.Set("DateTime", DateTime.Now.ToString());
                iServiceTemplate.Set("ServiceNamespace", codeGenerator.ServiceNamespace);

                String iServiceContent = iServiceTemplate.Render();

                var iServiceDocument = Path.Join(servicePath, "/", $"I{item.EntityName}Service.cs");
                NGFileContext.CreateFile(iServiceDocument);
                NGFileContext.WriteText(iServiceDocument, iServiceContent);

                #endregion

                #region 生成服务层实现

                var sqlServiceKey = "sqls:sql:template_service";
                var strServiceSQL = NGAdminGlobalContext.Configuration.GetSection(sqlServiceKey).Value;

                var serviceTemplate = new Template(strServiceSQL);

                serviceTemplate.Set("EntityName", item.EntityName);
                serviceTemplate.Set("Copyright", DateTime.Now.Year + " RuYiAdmin All Rights Reserved");
                serviceTemplate.Set("Version", Environment.Version.ToString());
                serviceTemplate.Set("Author", "auto generated by RuYiAdmin Template Code Generator");
                serviceTemplate.Set("DateTime", DateTime.Now.ToString());
                serviceTemplate.Set("ServiceNamespace", codeGenerator.ServiceNamespace);

                String serviceContent = serviceTemplate.Render();

                var serviceDocument = Path.Join(servicePath, "/", $"{item.EntityName}Service.cs");
                NGFileContext.CreateFile(serviceDocument);
                NGFileContext.WriteText(serviceDocument, serviceContent);

                #endregion

                #endregion

                #region 生成仓储层

                #region 生成仓储层接口

                var sqlIRepositoryKey = "sqls:sql:template_irepository";
                var strIRepositorySQL = NGAdminGlobalContext.Configuration.GetSection(sqlIRepositoryKey).Value;

                var iRepositoryTemplate = new Template(strIRepositorySQL);

                iRepositoryTemplate.Set("EntityName", item.EntityName);
                iRepositoryTemplate.Set("Copyright", DateTime.Now.Year + " RuYiAdmin All Rights Reserved");
                iRepositoryTemplate.Set("Version", Environment.Version.ToString());
                iRepositoryTemplate.Set("Author", "auto generated by RuYiAdmin Template Code Generator");
                iRepositoryTemplate.Set("DateTime", DateTime.Now.ToString());
                iRepositoryTemplate.Set("RepositoryNamespace", codeGenerator.RepositoryNamespace);

                String iRepositoryContent = iRepositoryTemplate.Render();

                var iRepositoryDocument = Path.Join(repositoryPath, "/", $"I{item.EntityName}Repository.cs");
                NGFileContext.CreateFile(iRepositoryDocument);
                NGFileContext.WriteText(iRepositoryDocument, iRepositoryContent);

                #endregion

                #region 生成仓储层实现

                var sqlRepositoryKey = "sqls:sql:template_repository";
                var strRepositorySQL = NGAdminGlobalContext.Configuration.GetSection(sqlRepositoryKey).Value;

                var repositoryTemplate = new Template(strRepositorySQL);

                repositoryTemplate.Set("EntityName", item.EntityName);
                repositoryTemplate.Set("Copyright", DateTime.Now.Year + " RuYiAdmin All Rights Reserved");
                repositoryTemplate.Set("Version", Environment.Version.ToString());
                repositoryTemplate.Set("Author", "auto generated by RuYiAdmin Template Code Generator");
                repositoryTemplate.Set("DateTime", DateTime.Now.ToString());
                repositoryTemplate.Set("RepositoryNamespace", codeGenerator.RepositoryNamespace);

                String repositoryContent = repositoryTemplate.Render();

                var repositoryDocument = Path.Join(repositoryPath, "/", $"{item.EntityName}Repository.cs");
                NGFileContext.CreateFile(repositoryDocument);
                NGFileContext.WriteText(repositoryDocument, repositoryContent);

                #endregion

                #endregion

                #region 生成视图层

                var sqlViewKey = String.Empty;

                switch (codeGenerator.LayoutMode)
                {
                    case 0:
                        sqlViewKey = "sqls:sql:template_view_layout_up_down";
                        break;
                    case 1:
                        sqlViewKey = "sqls:sql:template_view_layout_left_right";
                        break;
                    default:
                        sqlViewKey = "sqls:sql:template_view_layout_up_down";
                        break;
                }

                var strViewSQL = NGAdminGlobalContext.Configuration.GetSection(sqlViewKey).Value;

                var viewTemplate = new Template(strViewSQL);

                viewTemplate.Set("EntityName", item.EntityName);
                viewTemplate.Set("Copyright", DateTime.Now.Year + " RuYiAdmin All Rights Reserved");
                viewTemplate.Set("Version", Environment.Version.ToString());
                viewTemplate.Set("Author", "auto generated by RuYiAdmin Template Code Generator");
                viewTemplate.Set("DateTime", DateTime.Now.ToString());

                #region 处理Form字段

                properties.Clear();

                for (var i = 0; i < item.Fields.Count; i++)
                {
                    if (!item.Fields[i].IsFieldIgnoreCase())
                    {
                        properties.AppendLine($"{item.Fields[i].FieldName.ToLower()}: null,");
                    }
                }

                viewTemplate.Set("Fields", properties.ToString().TrimEnd('\r').TrimEnd('\n'));

                #endregion

                #region 处理Grid字段

                properties.Clear();

                for (var i = 0; i < item.Fields.Count; i++)
                {
                    if (!item.Fields[i].IsFieldIgnoreCase() && !item.Fields[i].FieldDataType.Equals("Guid"))
                    {
                        if (item.Fields[i].FieldDataType.Equals("int"))
                        {
                            if (item.Fields[i].FieldComment.Contains("0:") || item.Fields[i].FieldComment.Contains("0："))
                            {
                                #region 处理整型枚举

                                var arr = item.Fields[i].FieldComment.Split(',');
                                if (arr.Length == 1)
                                {
                                    arr = item.Fields[i].FieldComment.Split('，');
                                }

                                var kvs = this.GetKeyValueList(item.Fields[i]);

                                properties.AppendLine(
                                $"<el-table-column " +
                                $"label = '{arr[0]}' " +
                                $"prop = '{item.Fields[i].FieldName.ToLower()}' " +
                                $"align='center'" +
                                $" > " +
                                $"<template slot-scope='scope'>");

                                for (var m = 0; m < kvs.Count; m++)
                                {
                                    if (m == 0)
                                    {
                                        properties.AppendLine($"<el-tag v-if='scope.row.{item.Fields[i].FieldName.ToLower()} === {kvs[m].key}' type='success'>" +
                                            $"{kvs[m].value}" +
                                            $"</el-tag>");
                                    }
                                    else
                                    {
                                        properties.AppendLine($"<el-tag v-else-if='scope.row.{item.Fields[i].FieldName.ToLower()} === {kvs[m].key}' type='success'>" +
                                           $"{kvs[m].value}" +
                                           $"</el-tag>");
                                    }
                                }

                                properties.AppendLine($"</template>" +
                                    $"</el-table-column>");

                                #endregion
                            }
                        }
                        else
                        {
                            properties.AppendLine(
                                $"<el-table-column " +
                                $"label = '{item.Fields[i].FieldComment}' " +
                                $"prop = '{item.Fields[i].FieldName.ToLower()}' " +
                                $"align='center'" +
                                $" /> ");
                        }
                    }
                }

                viewTemplate.Set("GridColumns", properties.ToString().TrimEnd('\r').TrimEnd('\n'));

                #endregion

                #region 处理FormItems

                properties.Clear();

                var list = new List<DbSchemaField>();
                foreach (var field in item.Fields)
                {
                    if (!field.IsFieldIgnoreCase() && !field.FieldDataType.Equals("Guid"))
                    {
                        list.Add(field);
                    }
                }

                if (list.Count > 0)
                {
                    bool isOdd = (list.Count % 2 == 1) ? true : false;

                    for (var i = 0; i < list.Count; i++)
                    {
                        if (isOdd && i == list.Count - 1)
                        {
                            properties.AppendLine(
                                $"<el-row>" +
                                $"<el-col span='24'>" +
                                $"<el-form-item label='{this.GetFieldComment(list[i])}' prop='{list[i].FieldName.ToLower()}'>" +
                                this.GetInputControl(list[i]) +
                                $"</el-form-item>" +
                                $"</el-col>" +
                                $"</el-row>");
                        }
                        else
                        {
                            properties.AppendLine(
                                $"<el-row>" +
                                $"<el-col span='12'>" +
                                $"<el-form-item label='{this.GetFieldComment(list[i])}' prop='{list[i].FieldName.ToLower()}'>" +
                                this.GetInputControl(list[i]) +
                                $"</el-form-item>" +
                                $"</el-col>" +
                                $"<el-col span='12'>" +
                                $"<el-form-item label='{this.GetFieldComment(list[i + 1])}' prop='{list[i + 1].FieldName.ToLower()}'>" +
                                this.GetInputControl(list[i + 1]) +
                                $"</el-form-item>" +
                                $"</el-col>" +
                                $"</el-row>");
                            i++;
                        }
                    }
                }

                viewTemplate.Set("FormItems", properties.ToString().TrimEnd('\r').TrimEnd('\n'));

                #endregion

                #region 处理FormRules

                properties.Clear();

                if (list.Count > 0)
                {
                    foreach (var field in list)
                    {
                        if (field.IsNullable.ToUpper() == "NO")
                        {
                            String rule = String.Empty;
                            if (!String.IsNullOrEmpty(field.FieldMaxLength))
                            {
                                rule = String.Format("{0}: [【required: true,message: '请输入{1}',trigger: 'blur'】,【min: 0,max: {2},message: '最大长度{2}',trigger: 'blur'】],",
                                    field.FieldName.ToLower(), field.FieldComment, field.FieldMaxLength);
                            }
                            else
                            {
                                rule = String.Format("{0}: [【required: true,message: '请输入{1}',trigger: 'blur'】],",
                                    field.FieldName.ToLower(), field.FieldComment);
                            }

                            rule = rule.Replace("【", "{").Replace("】", "}");
                            properties.AppendLine(rule);
                        }
                    }
                }

                viewTemplate.Set("FormRules", properties.ToString().TrimEnd(',').TrimEnd('\r').TrimEnd('\n'));

                #endregion

                String viewContent = viewTemplate.Render();

                var viewDocument = Path.Join(viewPath, "/", $"{item.EntityName}Management.vue");
                NGFileContext.CreateFile(viewDocument);
                NGFileContext.WriteText(viewDocument, viewContent);

                #endregion
            }

            //制作zip压缩包
            ZipFile.CreateFromDirectory(tempPath, tempPath + ".zip", CompressionLevel.Fastest, false, Encoding.UTF8);

            return zipId;
        }

        #endregion

        #region 服务层私有方法

        #region 获取输入控件

        /// <summary>
        /// 获取输入控件
        /// </summary>
        /// <param name="field">DbSchemaField</param>
        /// <returns>前端控件</returns>
        private String GetInputControl(DbSchemaField field)
        {
            var result = String.Empty;

            switch (field.FieldDataType)
            {
                case "int":
                    if (field.FieldComment.Contains("0:") || field.FieldComment.Contains("0："))
                    {
                        #region 处理整型枚举

                        var arr = field.FieldComment.Split(',');
                        if (arr.Length == 1)
                        {
                            arr = field.FieldComment.Split('，');
                        }

                        var kvs = this.GetKeyValueList(field);

                        result = $"<el-select v-model='form.{field.FieldName.ToLower()}' placeholder='请选择{arr[0]}' filterable clearable > " +
                            $"<el-option v-for='item in {JsonConvert.SerializeObject(kvs).Replace("\"key\"", "key").Replace("\"value\"", "value")}'" +
                            $" :key='item.key' :label='item.value' :value='item.key'>" +
                            $"</el-option>" +
                            $"</el-select>";

                        #endregion
                    }
                    else
                    {
                        result = $"<el-input-number v-model='form.{field.FieldName.ToLower()}' " +
                        $"placeholder='请输入{field.FieldComment}' class='colWidth' />";
                    }
                    break;
                case "Double":
                case "float":
                case "decimal":
                    result = $"<el-input-number v-model='form.{field.FieldName.ToLower()}' " +
                        $"placeholder='请输入{field.FieldComment}' class='colWidth' />";
                    break;
                case "bool":
                    result = $"<el-radio-group v-model='form.{field.FieldName.ToLower()}'>" +
                        $"<el-radio :label='0'>备选项0</el-radio>" +
                        $"<el-radio :label='1'>备选项1</el-radio>" +
                        $"</el-radio-group>";
                    break;
                case "DateTime":
                    result = $"<el-date-picker v-model='form.{field.FieldName.ToLower()}' type='datetime' " +
                        $"placeholder='请选择{field.FieldComment}' class='colWidth' >" +
                        $"</el-date-picker>";
                    break;
                case "String":
                default:
                    result = $"<el-input v-model='form.{field.FieldName.ToLower()}' prefix-icon='el-icon-search' " +
                        $"placeholder='请输入{field.FieldComment}' class='colWidth' />";
                    break;
            }

            return result;
        }

        #endregion

        #region 获取KeyValue集合

        public class KeyValue
        {
            public object key { get; set; }

            public object value { get; set; }

            public KeyValue(object key, object value)
            {
                this.key = key;
                this.value = value;
            }
        }

        private List<KeyValue> GetKeyValueList(DbSchemaField field)
        {
            var kvs = new List<KeyValue>();

            var arr = field.FieldComment.Split(',');
            if (arr.Length == 1)
            {
                arr = field.FieldComment.Split('，');
            }

            foreach (var item in arr)
            {
                if (item.Contains(":") || item.Contains("："))
                {
                    var kv = item.Split(':');
                    if (kv.Length == 1)
                    {
                        kv = item.Split('：');
                    }

                    if (kv.Length == 1)
                    {
                        continue;
                    }

                    if (NGDigitUtil.IsInt(kv[0]))
                    {
                        kvs.Add(new KeyValue(int.Parse(kv[0].ToString()), kv[1]));
                    }
                }
            }

            return kvs;
        }

        #endregion

        #region 获取字段说明

        private String GetFieldComment(DbSchemaField field)
        {
            var result = String.Empty;

            if (field.FieldComment.Contains("0:") || field.FieldComment.Contains("0："))
            {
                var arr = field.FieldComment.Split(',');
                if (arr.Length == 1)
                {
                    arr = field.FieldComment.Split('，');
                }

                result = arr[0];
            }
            else
            {
                result = field.FieldComment;
            }

            return result;
        }

        #endregion

        #endregion
    }
}
