using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel
{
    public static class HierarchyNodeExtensions
    {
        /// <summary>
        /// Adds the specified parent.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="kod">The kod.</param>
        /// <param name="nazov">The nazov.</param>
        /// <returns></returns>
        public static HierarchyNode Add(this HierarchyNode parent, string kod, string nazov, string typ = HierarchyNodeType.DatovaPolozka)
        {
            var child = new HierarchyNode(kod, nazov, null, null, typ);
            child.Parent = parent;
            parent.Children.Add(child);
            return child;
        }

        /// <summary>
        /// Toes the caption.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToCaption(this System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attributes = (PfeCaptionAttribute[])fi.GetCustomAttributes(typeof(PfeCaptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Caption : value.ToString();
        }

        public static Dictionary<string, object> ParametrizedDbFilter(this HierarchyNode node)
        {
            return ParametrizedDbFilter(node, null);
        }

        private static Dictionary<string, object> ParametrizedDbFilter(HierarchyNode node, Dictionary<string, object> dict)
        {
            if (node is DatabaseHierarchyNode)
            {
                if (node.Parameter != null)
                {
                    PropertyInfo prop = null;
                    if (node.ModelType.GetProperties().Any(nav => nav.HasAttribute<HierarchyNodeParameterAttribute>()))
                    {

                        prop = node.ModelType.GetProperties().First(nav => nav.HasAttribute<HierarchyNodeParameterAttribute>());

                    }
                    else
                    {
                        prop = node.ModelType.GetProperties().First(nav => nav.HasAttribute<PrimaryKeyAttribute>());
                    }

                    string name = prop.HasAttribute<AliasAttribute>() ? prop.FirstAttribute<AliasAttribute>().Name : prop.Name;
                    if (dict == null)
                        dict = new Dictionary<string, object>();
                    dict.Add(name, node.Parameter);
                }
            }

            if (node.Parent != null)
            {
                return ParametrizedDbFilter(node.Parent, dict);
            }

            return dict;
        }

        /// <summary>
        /// Filters the by access.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="repository">The repository.</param>
        /// <returns></returns>
        public static List<NodeAction> FilterByAccess(this List<NodeAction> list, IWebEasRepositoryBase repository)
        {
            var sessionRoles = repository.Session.Roles;
            var curActions = new List<NodeAction>();

            foreach (NodeAction act in list)
            {
                if (HierarchyNode.HasRolePrivileges(act, sessionRoles))
                {
                    curActions.Add(act);
                }
            }

            return curActions;
        }

        /// <summary>
        /// Finds the specified nodes.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="kodPolozky">The kod polozky.</param>
        /// <returns></returns>
        public static HierarchyNode Find(this List<HierarchyNode> nodes, string kodPolozky)
        {
            if (string.IsNullOrEmpty(kodPolozky))
            {
                return null;
            }

            kodPolozky = kodPolozky.ToLower();
            bool into = kodPolozky.Contains("-");
            string first = into ? kodPolozky.Substring(0, kodPolozky.IndexOf('-')) : kodPolozky;            
            bool hasData = first.Contains('!');
            string data = null;
            if (hasData)
            {
                string[] splitted = first.Split('!');
                first = splitted[0];
                data = splitted[1];
            }

            //prvy pokus - explicitne dohladana polozka (na cross-modulove polozky sa to nechyti)
            foreach (HierarchyNode node in nodes)
            {
                //Podpora pre polozky stromu, ktore su CROSS moduly. Na zaciatku maju all- a modul je vlozeny za vykricnik na koniec
                if (node.Kod == first || first == "all" && node.Kod == kodPolozky.Substring(kodPolozky.Length - 3))
                {
                    HierarchyNode item = node.Clone();                    
                    if (hasData)
                    {
                        item.Parameter = data;
                    }

                    if (into)
                    {
                        return item.Find(kodPolozky);
                    }

                    return item;
                }
            }

            //Druhy pokus - dohladanie cross-modulovych poloziek. ajskor hladam v REG, potom v ostatnych
            if (first == "all" && into)
            {
                HierarchyNode hn = null;

                //Prve kolo - hladam iba REG
                //Hladam v module REG, tento modul je urceny pri cross.modulovych polozkach na zadefinovanie polozky bez fitra na modul
                foreach (HierarchyNode node in nodes)
                {
                    if (node.Kod == "reg")
                    {
                        hn = node.Clone().Find(kodPolozky);
                        if (hn != null) return hn;
                    }
                }

                //Druhe kolo - hladam v ostatnych moduloch okrem REG
                foreach (HierarchyNode node in nodes)
                {
                    if (node.Kod != "reg")
                    {
                        hn = node.Clone().Find(kodPolozky);
                        if (hn != null) return hn;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the specified nodes.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="typEntity">The TypBiznisEntityEnum value.</param>
        /// <returns></returns>
        public static IEnumerable<HierarchyNode> Find(this List<HierarchyNode> nodes, WebEas.ServiceModel.Office.Egov.Reg.Types.TypBiznisEntityEnum typEntity)
        {
            foreach (HierarchyNode node in nodes)
            {
                if (node.TyBiznisEntity != null && node.TyBiznisEntity.Contains(typEntity))
                    yield return node;
                if (node.HasChildren)
                    foreach (var childNode in Find(node.Children, typEntity))
                        yield return childNode;
            }
        }

        /// <summary>
        /// Finds the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="kodPolozky">The kod polozky.</param>
        /// <returns></returns>
        public static HierarchyNode Find(this HierarchyNode node, string kodPolozky)
        {
            string first = kodPolozky.Remove(0, node.KodPolozky.Length + 1);

            bool into = first.Contains('-');
            string level = into ? first.Substring(0, first.IndexOf('-')) : first;
            bool hasData = level.Contains('!');
            string data = null;
            if (hasData)
            {
                string[] splitted = level.Split('!');
                level = splitted[0];
                data = splitted[1];
            }

            foreach (HierarchyNode child in node.Children)
            {
                if (child.Kod == level)
                {
                    HierarchyNode item = child.Clone();
                    item.Parent = node;
                    if (hasData)
                    {
                        item.Parameter = data;
                    }

                    if (into)
                    {
                        return item.Find(kodPolozky);
                    }

                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Firsts the specified nodes.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="typ">The typ.</param>
        /// <returns></returns>
        public static HierarchyNode First(this List<HierarchyNode> nodes, Type typ)
        {
            if (typ == null)
            {
                return null;
            }

            HierarchyNode founded;

            foreach (HierarchyNode node in nodes)
            {
                if (node.First(typ, out founded))
                {
                    return founded;
                }
            }
            return null;
        }

        /// <summary>
        /// Firsts the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="typ">The typ.</param>
        /// <param name="founded">The founded.</param>
        /// <returns></returns>
        public static bool First(this HierarchyNode node, Type typ, out HierarchyNode founded)
        {
            founded = null;

            if (node.ModelType.Name.Equals(typ.Name) || node.ModelType.Name.Equals(string.Format("{0}View", typ.Name)))
            {
                founded = node;
                return true;
            }

            if (!node.Children.IsNullOrEmpty())
            {
                foreach (HierarchyNode child in node.Children)
                {
                    if (child.First(typ, out founded))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Finds the specified nodes.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="typ">The typ.</param>
        /// <returns></returns>
        public static List<HierarchyNode> Find(this List<HierarchyNode> nodes, Type typ, List<HierarchyNode> foundNodes = null)
        {
            if (typ == null)
            {
                return null;
            }

            if (foundNodes == null)
            {
                foundNodes = new List<HierarchyNode>();
            }

            foreach (HierarchyNode node in nodes)
            {
                foundNodes.AddRange(node.Find(typ));
            }

            return foundNodes;
        }

        /// <summary>
        /// Finds the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="typ">The typ.</param>
        /// <returns></returns>
        public static List<HierarchyNode> Find(this HierarchyNode node, Type typ, List<HierarchyNode> foundNodes = null)
        {
            if (foundNodes == null)
            {
                foundNodes = new List<HierarchyNode>();
            }

            if (node.ModelType.Name.Equals(typ.Name) || node.ModelType.Name.Equals(string.Format("{0}View", typ.Name)))
            {
                foundNodes.Add(node);
            }

            if (!node.Children.IsNullOrEmpty())
            {
                foreach (HierarchyNode child in node.Children)
                {
                    foundNodes.AddRange(child.Find(typ));
                }
            }

            return foundNodes;
        }

        /// <summary>
        /// Gets the data model.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="configuredData">The configured data.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static PfeDataModel GetDataModel(this HierarchyNode node, IWebEasRepositoryBase repository, IPohlad configuredData = null, PfeModelType? type = null, string filterString = null)
        {
            if (node.ModelType == null)
            {
                throw new Exception(String.Format("Model not defined for {0}", node.KodPolozky));
            }

            //if (type == PfeModelType.Form)
            //{
            //    return configuredData.Data;
            //}

            var model = new PfeDataModel();

            //Enum.TryParse<DataModel.ModelType>(type, out viewType);

            PfeDataModelAttribute defDataModel = node.ModelType.FirstAttribute<PfeDataModelAttribute>();
            //Id
            model.Id = configuredData == null ? 0 : configuredData.Id;
            // Nazov modelu
            model.Name = configuredData == null ? defDataModel == null || String.IsNullOrEmpty(defDataModel.Name) ? node.Nazov : defDataModel.Name : configuredData.Nazov;
            // Typ modelu

            PfeModelType typ;

            if (configuredData != null && !String.IsNullOrEmpty(configuredData.Typ) && Enum.TryParse<PfeModelType>(configuredData.Typ.ToTitleCase(), out typ))
            {
                model.Type = typ;
            }
            else
            {
                model.Type = defDataModel != null && type == null ? defDataModel.Type : type ?? PfeModelType.Grid;
            }
            // Show in actions
            if (configuredData != null)
            {
                model.ShowInActions = configuredData.ShowInActions;
            }

            // selection z hierarchy node            
            model.SelectionMode = node.SelectionMode;

            // Stlpce modelu

            bool hasPrimary = node.ModelType.GetProperties().Any(nav => nav.DeclaringType == node.ModelType && nav.HasAttribute<PrimaryKeyAttribute>());

            //bool hasPfeDataValue = node.ModelType

            foreach (PropertyInfo property in node.ModelType.GetProperties())
            {
                if (property.HasAttribute<PfeIgnoreAttribute>() || property.HasAttribute<IgnoreDataMemberAttribute>())
                {
                    continue;
                }

                // Nazov
                PfeColumnAttribute defCol = property.FirstAttribute<PfeColumnAttribute>() ?? new PfeColumnAttribute();
                defCol.PropertyTypeInfo = property;

                if (String.IsNullOrEmpty(defCol.Name))
                {
                    if (property.HasAttribute<DataMemberAttribute>())
                    {
                        defCol.Name = property.FirstAttribute<DataMemberAttribute>().Name;
                        if (String.IsNullOrEmpty(defCol.Name))
                        {
                            defCol.Name = property.Name;
                        }
                    }
                    else if (String.IsNullOrEmpty(defCol.Name))
                    {
                        defCol.Name = property.Name;
                    }
                }

                //Primary key
                if ((hasPrimary && property.DeclaringType == node.ModelType && property.HasAttribute<PrimaryKeyAttribute>()) || (!hasPrimary && property.HasAttribute<PrimaryKeyAttribute>()))
                {
                    if (String.IsNullOrEmpty(defCol.Text))
                    {
                        defCol.Text = "_Id";
                    }

                    defCol.Hidden = true;
                    defCol.Hideable = false;
                    defCol.Rank = 1;
                    defCol.IsPrimary = true;
                }
                else if (defCol.Name.EndsWith("_Id"))
                {
                    if (!defCol.HasHidden)
                    {
                        defCol.Hidden = true;
                    }
                    if (!defCol.HasHideable)
                    {
                        defCol.Hideable = false;
                    }
                }

                // Text
                if (defCol.Hidden && !defCol.Hideable)
                {
                    if (String.IsNullOrEmpty(defCol.Text))
                    {
                        defCol.Text = string.Format("_{0}", property.Name);
                    }
                    else if (!defCol.Text.StartsWith("_"))
                    {
                        defCol.Text = string.Format("_{0}", defCol.Text);
                    }
                }
                else if (String.IsNullOrEmpty(defCol.Text))
                {
                    defCol.Text = property.Name;
                }

                //Typ
                if (defCol.Type == PfeDataType.Unknown)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        defCol.Type = PfeDataType.Text;
                    }
                    else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                    {
                        defCol.Type = PfeDataType.Boolean;
                    }
                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    {
                        defCol.Type = PfeDataType.DateTime;
                    }
                    else if (property.PropertyType == typeof(TimeSpan) || property.PropertyType == typeof(TimeSpan?))
                    {
                        defCol.Type = PfeDataType.Time;
                    }
                    else if (property.PropertyType == typeof(short) || property.PropertyType == typeof(short?) || 
                             property.PropertyType == typeof(int) || property.PropertyType == typeof(int?) || 
                             property.PropertyType == typeof(long) || property.PropertyType == typeof(long?) || 
                             property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?) ||
                             property.PropertyType == typeof(byte) || property.PropertyType == typeof(byte?))
                    {
                        defCol.Type = PfeDataType.Number;
                    }
                    else
                    {
                        defCol.Type = PfeDataType.Text;
                    }
                }

                //XType
                if (defCol.Xtype == PfeXType.Unknown)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        if (property.HasAttribute<PfeComboAttribute>())
                        {
                            string nameField = defCol.Name;
                            if (property.HasAttribute<PfeComboAttribute>())
                            {
                                PfeComboAttribute atr = property.FirstAttribute<PfeComboAttribute>();
                                if (!String.IsNullOrEmpty(atr.NameColumn))
                                {
                                    nameField = atr.NameColumn;
                                }
                                else
                                {
                                    nameField = atr.IdColumnCombo;
                                }
                            }
                            defCol.Xtype = PfeXType.Combobox;
                            defCol.IdField = "id";
                            defCol.ValueField = "value";
                            defCol.NameField = nameField;
                            defCol.DataUrl = String.Format("/office/{0}/combo/{1}/{2}", node.KodRoot, node.KodPolozky, property.Name.ToLower());
                        }
                        else
                        {
                            defCol.Xtype = PfeXType.Textfield;
                        }
                    }
                    else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                    {
                        defCol.Xtype = PfeXType.Checkbox;
                    }
                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    {
                        defCol.Xtype = PfeXType.Datefield;
                    }
                    else if (property.PropertyType == typeof(TimeSpan) || property.PropertyType == typeof(TimeSpan?))
                    {
                        defCol.Xtype = PfeXType.Datefield;
                    }
                    else if (property.PropertyType == typeof(short) || property.PropertyType == typeof(short?) || 
                             property.PropertyType == typeof(int) || property.PropertyType == typeof(int?) || 
                             property.PropertyType == typeof(long) || property.PropertyType == typeof(long?) || 
                             property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?) ||
                             property.PropertyType == typeof(byte) || property.PropertyType == typeof(byte?))
                    {
                        if (property.HasAttribute<PfeComboAttribute>())
                        {
                            string nameField = defCol.Name;
                            if (property.HasAttribute<PfeComboAttribute>())
                            {
                                PfeComboAttribute atr = property.FirstAttribute<PfeComboAttribute>();
                                if (!String.IsNullOrEmpty(atr.NameColumn))
                                {
                                    nameField = atr.NameColumn;
                                }
                                else
                                {
                                    nameField = atr.IdColumnCombo;
                                }
                            }

                            defCol.Xtype = PfeXType.Combobox;
                            defCol.IdField = "id";
                            defCol.ValueField = "value";
                            defCol.NameField = nameField;
                            defCol.DataUrl = String.Format("/office/{0}/combo/{1}/{2}", node.KodRoot, node.KodPolozky, property.Name.ToLower());
                        }
                        else
                        {
                            defCol.Xtype = PfeXType.Numberfield;
                        }
                    }
                    else
                    {
                        defCol.Xtype = PfeXType.Textfield;
                    }
                }
                //Align
                if (defCol.Align == PfeAligment.Unknown)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        defCol.Align = PfeAligment.Left;
                    }
                    else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                    {
                        defCol.Align = PfeAligment.Center;
                    }
                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?) || property.PropertyType == typeof(TimeSpan) || property.PropertyType == typeof(TimeSpan?))
                    {
                        defCol.Align = PfeAligment.Right;
                    }
                    else if (property.PropertyType == typeof(short) || property.PropertyType == typeof(short?) || 
                             property.PropertyType == typeof(int) || property.PropertyType == typeof(int?) || 
                             property.PropertyType == typeof(long) || property.PropertyType == typeof(long?) || 
                             property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?) ||
                             property.PropertyType == typeof(byte) || property.PropertyType == typeof(byte?))
                    {
                        if (property.HasAttribute<ForeignKeyAttribute>())
                        {
                            defCol.Align = PfeAligment.Default;
                        }
                        else
                        {
                            defCol.Align = PfeAligment.Right;
                        }
                    }
                    else
                    {
                        defCol.Align = PfeAligment.Left;
                    }
                }

                //Format
                if (String.IsNullOrEmpty(defCol.Format))
                {
                    if (property.PropertyType == typeof(short) || property.PropertyType == typeof(short?) ||
                        property.PropertyType == typeof(int) || property.PropertyType == typeof(int?) ||
                        property.PropertyType == typeof(long) || property.PropertyType == typeof(long?) ||
                        property.PropertyType == typeof(byte) || property.PropertyType == typeof(byte?))
                    {
                        if (defCol.Name.ToLower().Contains("rok") || property.Name.ToLower().Contains("rok"))
                        {
                            defCol.Format = "0";
                        }
                        else
                        {
                            defCol.Format = "0,000";
                        }
                    }
                    else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                    {
                        defCol.Format = "0,000.00##";
                    }
                }

                //MaxLength
                if (!defCol.MaxLength.HasValue)
                {
                    if (property.HasAttribute<ServiceStack.DataAnnotations.StringLengthAttribute>())
                    {
                        defCol.MaxLength = property.FirstAttribute<ServiceStack.DataAnnotations.StringLengthAttribute>().MaximumLength;
                    }
                }

                // Is Decimal
                if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                {
                    defCol.IsDecimal = true;
                }
                else
                {
                    defCol.IsDecimal = false;
                }

                //ReadOnly
                if (!defCol.HasReadOnly && !string.IsNullOrEmpty(defCol.NameField))
                {
                    defCol.ReadOnly = true;
                }

                //Editable
                if (defCol.ReadOnly && !defCol.HasEditable && (string.IsNullOrEmpty(defCol.NameField)))
                {
                    defCol.Editable = false;
                }

                //Mandatory
                if (!defCol.HasMandatory)
                {
                    if (property.HasAttribute<ServiceStack.DataAnnotations.RequiredAttribute>() || property.HasAttribute<System.ComponentModel.DataAnnotations.RequiredAttribute>())
                    {
                        if (defCol.Editable)
                        {
                            defCol.Mandatory = true;
                        }
                    }
                    else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(DateTime) || 
                             property.PropertyType == typeof(byte) || property.PropertyType == typeof(short) || 
                             property.PropertyType == typeof(int) || property.PropertyType == typeof(long) || 
                             property.PropertyType == typeof(decimal) || property.PropertyType == typeof(Guid))
                    {
                        if (defCol.Editable)
                        {
                            defCol.Mandatory = true;
                        }
                    }
                }

                NodeFieldDefaultValue nodeFieldDefaultValue = node.DefaultValues.FirstOrDefault(x => x.FieldName == property.Name);
                if (nodeFieldDefaultValue != null)
                {
                    defCol.DefaultValue = nodeFieldDefaultValue.DefaultValue;
                }

                KeyValuePair<string, bool> nodeSystemColumn = node.SystemColumns.FirstOrDefault(x => x.Key == property.Name);
                if (!string.IsNullOrEmpty(nodeSystemColumn.Key))
                {
                    defCol.Hidden = nodeSystemColumn.Value;
                    defCol.Hideable = false;
                }

                // readonly
                if (!defCol.HasReadOnly && !string.IsNullOrEmpty(defCol.NameField))
                {
                    defCol.ReadOnly = true;
                }

                if (defCol.ReadOnly && !defCol.HasEditable && string.IsNullOrEmpty(defCol.NameField))
                {
                    defCol.Editable = false;
                }

                //Zrusenie comba ak nie je editovatelne - nemozeme, lebo chceme aby pri filtrovani to kombo bolo...
                //if (!defCol.Editable && defCol.Xtype == PfeXType.Combobox && !property.HasAttribute<PfeFilterAttribute>())
                //{
                //	defCol.Xtype = PfeXType.Textfield;
                //	defCol.IdField = null;
                //	defCol.ValueField = null;
                //	defCol.NameField = null;
                //	defCol.DataUrl = null;                    
                //}

                model.Fields.Add(defCol);

                if (property.HasAttribute<PfeSortAttribute>())
                {
                    foreach (PfeSortAttribute sort in property.AllAttributes<PfeSortAttribute>())
                    {
                        if (String.IsNullOrEmpty(sort.Field))
                        {
                            sort.Field = defCol.Name;
                        }
                        model.MultiSort.Add(sort);
                    }
                }

                if (property.HasAttribute<PfeGroupAttribute>())
                {
                    foreach (PfeGroupAttribute group in property.AllAttributes<PfeGroupAttribute>())
                    {
                        if (String.IsNullOrEmpty(group.Field))
                        {
                            group.Field = defCol.Name;
                        }
                        model.FieldGroups.Add(group);
                    }
                }

                if (property.HasAttribute<PfeFilterAttribute>())
                {
                    foreach (PfeFilterAttribute filter in property.AllAttributes<PfeFilterAttribute>())
                    {
                        if (String.IsNullOrEmpty(filter.Field))
                        {
                            filter.Field = defCol.Name;
                        }
                        model.Filters.Add(filter);
                    }
                }
            }

            // Nastavovanie comboboxov
            foreach (PfeColumnAttribute attribute in model.Fields)
            {
                // Nastavenie atributu idckoveho stlpca Editable=false ak je takto nastaveny combo stlpec, ktory ho referencuje
                {
                    PfeColumnAttribute refColumn = model.Fields.Where(x => x.PropertyTypeInfo.HasAttribute<PfeComboAttribute>() &&
                                                                           x.PropertyTypeInfo.FirstAttribute<PfeComboAttribute>().NameColumn == attribute.Name).FirstOrDefault();

                    if (refColumn != null)
                    {
                        if (!refColumn.Editable)
                        {
                            attribute.Editable = refColumn.Editable;
                        }
                    }
                }

                // Nastavenie atributu Mandatory=true ak ma idckovy stlpec Mandatory=true
                if (attribute.PropertyTypeInfo.HasAttribute<PfeComboAttribute>())
                {
                    PfeColumnAttribute idColumn = model.Fields.Where(x => x.Name == attribute.PropertyTypeInfo.FirstAttribute<PfeComboAttribute>().NameColumn).FirstOrDefault();

                    if (idColumn != null)
                    {
                        if (idColumn.Mandatory)
                        {
                            attribute.Mandatory = idColumn.Mandatory;
                        }
                    }
                }
            }

            if (model.Type == PfeModelType.Form)
            {
                model.Pages = new List<PfePage>()
                {
                    new PfePage("Obsah", new List<PfeRow>()
                    {
                        new PfeRow(new List<PfePageFieldGroup>()
                        {
                            new PfePageFieldGroup("Obsah", (from m in model.Fields
                                                            where m.Hidden == false
                                                            select m.Name).ToList())
                        })
                    }),
                    new PfePage("Doplňujúce údaje", new List<PfeRow>()
                    {
                        new PfeRow(new List<PfePageFieldGroup>()
                        {
                            new PfePageFieldGroup("Doplňujúce údaje", new List<string>() { "Poznamka" })
                        })
                    })
                };
                model.FieldGroups = null;
                model.Filters = null;
                model.MultiSort = null;
                model.Layout = null;
            }

            if (model.Type == PfeModelType.Layout)
            {
                model.Fields = null;
                model.FieldGroups = null;
                model.Filters = null;
                model.MultiSort = null;
                model.Pages = null;
            }

            if (configuredData != null && !string.IsNullOrEmpty(configuredData.Data))
            {
                //Spojenie
                model = model.Merge(ServiceStack.Text.JsonSerializer.DeserializeFromString<PfeDataModel>(configuredData.Data));
            }

            if (model.Fields != null && model.Fields.Count > 1)
            {
                if (model.Fields.Any(nav => nav.Rank == 0))
                {
                    model.Fields = model.Fields.OrderByDescending(nav => nav.IsPrimary).ThenBy(nav => nav.Hidden).ThenByDescending(nav => nav.Hideable).ThenBy(nav => nav.Rank > 0 ? nav.Rank : 1000).ToList();
                    int start = 0;

                    foreach (PfeColumnAttribute col in model.Fields)
                    {
                        col.Rank = ++start;
                    }
                }
                else
                {
                    model.Fields = model.Fields.OrderBy(nav => nav.Rank).ToList();
                }
            }

            try
            {
                if (typeof(IPfeCustomize).IsAssignableFrom(node.ModelType) || typeof(IPfeCustomizeDefaultValue).IsAssignableFrom(node.ModelType))
                {
                    object modelObject = Activator.CreateInstance(node.ModelType);

                    if (modelObject is IPfeCustomizeDefaultValue)
                    {
                        foreach (PfeColumnAttribute field in model.Fields)
                        {
                            ((IPfeCustomizeDefaultValue)modelObject).CustomizeDefaultValue(field, repository, node);
                        }
                    }
                    if (modelObject is IPfeCustomize)
                    {
                        ((IPfeCustomize)modelObject).CustomizeModel(model, repository, node, filterString);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new WebEasException("Customize Pfe", ex);
            }

            return model;
        }

        /// <summary>
        /// Adds the default actions.
        /// </summary>
        /// <param name="typ">The typ.</param>
        /// <param name="existingActions">The existing actions.</param>
        public static void AddDefaultActions(this HierarchyNode node, List<NodeAction> existing)
        {
            //ak je tam declared only tak nenajde primary key v base triede - napr. pri pouziti View triedy ktora ma len cis. hodnoty a dedi od normalnej entity
            //var properties = node.ModelType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] properties = node.ModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string idField = null;

            if (properties.Any(nav => nav.HasAttribute<PrimaryKeyAttribute>()))
            {
                PropertyInfo property = properties.First(nav => nav.HasAttribute<PrimaryKeyAttribute>());
                idField = property.HasAttribute<DataMemberAttribute>() ? property.FirstAttribute<DataMemberAttribute>().Name ?? property.Name : property.Name;
            }

            if (!existing.Any(nav => nav.ActionType == NodeActionType.ReadList))
            {
                existing.Insert(0, new NodeAction(NodeActionType.ReadList)
                {
                    Node = node,
                    IdField = idField
                });
            }

            foreach (NodeAction act in existing.Where(nav => nav.ActionType != NodeActionType.ReadList))
            {
                if (String.IsNullOrEmpty(act.IdField))
                {
                    act.IdField = idField;
                }

                if (act.ActionType == NodeActionType.MenuButtons)
                {
                    foreach (NodeAction menuAct in act.MenuButtons)
                    {
                        if (String.IsNullOrEmpty(menuAct.IdField))
                        {
                            menuAct.IdField = idField;
                        }

                        if (String.IsNullOrEmpty(menuAct.Url))
                        {
                            menuAct.Url = GetUrl(menuAct, node.ModelType);
                        }
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(act.Url))
                    {
                        act.Url = GetUrl(act, node.ModelType);
                    }
                }
            }
        }

        /// <summary>
        /// Get root node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static HierarchyNode GetRootNode(this HierarchyNode node)
        {
            if (node.IsRoot)
            {
                return node;
            }
            else
            {
                return node.Parent.GetRootNode();
            }
        }

        /// <summary>
        /// Return true if user has access for node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="userRoles"></param>
        /// <returns></returns>
        public static bool IsInRole(this HierarchyNode node, HashSet<string> userRoles)
        {
            // ak nema ziadnu rolu, povolime pristup
            if (node.Roles == null || node.Roles.Count == 0)
            {
                return true;
            }

            foreach (string role in node.Roles)
            {
                if (userRoles.Contains(role))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the start URL.
        /// </summary>
        /// <param name="typ">The typ.</param>
        /// <returns></returns>
        internal static string GetStartUrl(Type typ)
        {
            var sb = new StringBuilder();

            string name = typ.Namespace.ToLower();
            if (name.Contains(".office"))
            {
                sb.Append("/office/");
                sb.Append(name.Substring(name.IndexOf(".office.") + 8, 3));
            }
            else if (name.Contains(".pfe"))
            {
                sb.Append("/pfe");
            }
            else if (name.Contains(".osoba"))
            {
                sb.Append("/private");
            }
            else if (name.Contains(".private"))
            {
                sb.Append("/private");
            }
            else if (name.Contains(".formulare"))
            {
                sb.Append("/formulare");
            }
            else if (name.Contains(".public"))
            {
                sb.Append("/public");
            }

            //if (typ.HasAttribute<SchemaAttribute>())
            //{
            //    sb.Append("/");
            //    sb.Append(typ.FirstAttribute<SchemaAttribute>().Name.ToLower());
            //}

            //if (typ.HasAttribute<AliasAttribute>())
            //{
            //    string alias = typ.FirstAttribute<AliasAttribute>().Name;
            //    if (alias.ToLower().StartsWith("c_"))
            //    {
            //        sb.Append("/cis");
            //    }
            //}
            return sb.ToString();
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="typ">The typ.</param>
        /// <returns></returns>
        private static string GetUrl(NodeAction action, Type typ)
        {
            if (!String.IsNullOrEmpty(action.Path))
            {
                return action.Path;// string.Format("{0}{1}", GetStartUrl(typ), action.Path);
                //return string.Format("{0}/{1}{2}", GetStartUrl(typ), action.Type.ToString(), typ.Name.Replace("View", ""));
            }
            return null;
        }

        public static HierarchyNode SetLayoutDependencies(this HierarchyNode node, params HierarchyNodeDependency[] dependencies)
        {
            if (dependencies == null || dependencies.Length == 0) return node;

            //generate dictionary for quick access
            Dictionary<string, HierarchyNode> table = new Dictionary<string,HierarchyNode>(50);
            FillNodeTable(node, table);

            foreach (var dependency in dependencies)
                dependency.ApplyToNodes(table);

            return node;
        }

        private static void FillNodeTable(HierarchyNode node, Dictionary<string,HierarchyNode> table)
        {
            if (node.HasData)
                table.Add(node.KodPolozky, node);
            foreach (var child in node.Children)
                FillNodeTable(child, table);
        }
    }

    public class HierarchyNodeDependency
    {
        string[] masterCode;
        string masterColumn;
        string detailCode;
        string detailColumn;
        string masterDescription;
        string detailDescription;
        int masterType;
        int detailType;

        private HierarchyNodeDependency(string[] masterCode, string detailCode, string masterColumn, string detailColumn, string master2detailName, string detail2masterName, int masterType, int detailType)
        {
            this.masterCode = masterCode;
            this.masterColumn = masterColumn;
            this.detailCode = detailCode;
            this.detailColumn = detailColumn;
            this.masterDescription = detail2masterName;
            this.detailDescription = master2detailName;
            this.masterType = masterType;
            this.detailType = detailType;
        }

        public static HierarchyNodeDependency One2ManyBack2One(string masterCode, string detailCode, string column, string master2detailName, string detail2masterName)
        {
            return new HierarchyNodeDependency(new string[] { masterCode }, detailCode, column, column, master2detailName, detail2masterName, (int)PfeRelationType.OneToMany, (int)PfeRelationType.OneToOne);
        }

        public static HierarchyNodeDependency One2ManyBack2One(string masterCode, string detailCode, string masterColumn, string detailColumn, string master2detailName, string detail2masterName)
        {
            return new HierarchyNodeDependency(new string[] { masterCode }, detailCode, masterColumn, detailColumn, master2detailName, detail2masterName, (int)PfeRelationType.OneToMany, (int)PfeRelationType.OneToOne);
        }

        public static HierarchyNodeDependency One2OneAndBack(string masterCode, string detailCode, string column, string master2detailName, string detail2masterName)
        {
            return new HierarchyNodeDependency(new string[] { masterCode }, detailCode, column, column, master2detailName, detail2masterName, (int)PfeRelationType.OneToOne, (int)PfeRelationType.OneToOne);
        }

        public static HierarchyNodeDependency One2OneAndBack(string masterCode, string detailCode, string masterColumn, string detailColumn, string master2detailName, string detail2masterName)
        {
            return new HierarchyNodeDependency(new string[] { masterCode }, detailCode, masterColumn, detailColumn, master2detailName, detail2masterName, (int)PfeRelationType.OneToOne, (int)PfeRelationType.OneToOne);
        }

        public static HierarchyNodeDependency One2ManyNoBack(string masterCode, string detailCode, string column, string master2detailName)
        {
            return new HierarchyNodeDependency(new string[] { masterCode }, detailCode, column, column, master2detailName, null, (int)PfeRelationType.OneToMany, 0);
        }

        public static HierarchyNodeDependency One2ManyNoBack(string masterCode, string detailCode, string masterColumn, string detailColumn, string master2detailName)
        {
            return new HierarchyNodeDependency(new string[] { masterCode }, detailCode, masterColumn, detailColumn, master2detailName, null, (int)PfeRelationType.OneToMany, 0);
        }

        public static HierarchyNodeDependency One2OneNoBack(string masterCode, string detailCode, string column, string master2detailName)
        {
            return new HierarchyNodeDependency(new string[] { masterCode }, detailCode, column, column, master2detailName, null, (int)PfeRelationType.OneToOne, 0);
        }

        public static HierarchyNodeDependency One2OneNoBack(string masterCode, string detailCode, string masterColumn, string detailColumn, string master2detailName)
        {
            return new HierarchyNodeDependency(new string[] { masterCode }, detailCode, masterColumn, detailColumn, master2detailName, null, (int)PfeRelationType.OneToOne, 0);
        }

        public static HierarchyNodeDependency MultipleOne2ManyNoBack(string[] masterCodes, string detailCode, string column, string master2detailName)
        {
            return new HierarchyNodeDependency(masterCodes, detailCode, column, column, master2detailName, null, (int)PfeRelationType.OneToMany, 0);
        }

        public static HierarchyNodeDependency MultipleOne2ManyNoBack(string[] masterCodes, string detailCode, string masterColumn, string detailColumn, string master2detailName)
        {
            return new HierarchyNodeDependency(masterCodes, detailCode, masterColumn, detailColumn, master2detailName, null, (int)PfeRelationType.OneToMany, 0);
        }

        public void ApplyToNodes(Dictionary<string, HierarchyNode> table)
        {
            for (int i=0; i<masterCode.Length; i++)
            {
                HierarchyNode node;
                if (!table.TryGetValue(masterCode[i], out node)) continue;

                if (node.LayoutDependencies == null) node.LayoutDependencies = new List<LayoutDependency>();
                node.LayoutDependencies.Add(new LayoutDependency(detailCode, new List<LayoutDependencyRelations> {
                    new LayoutDependencyRelations() {
                        MasterField = masterColumn,
                        DetailField = detailColumn,
                        LinkDescription = detailDescription,
                        RelationType = (PfeRelationType)masterType
                    }
                }));
                if (detailType > 0)
                {
                    node = table[detailCode];
                    if (node.LayoutDependencies == null) node.LayoutDependencies = new List<LayoutDependency>();
                    node.LayoutDependencies.Add(new LayoutDependency(masterCode[i], new List<LayoutDependencyRelations> {
                        new LayoutDependencyRelations() {
                            MasterField = detailColumn,
                            DetailField = masterColumn,
                            LinkDescription = masterDescription,
                            RelationType = (PfeRelationType)detailType
                        }
                    }));
                }
            }
        }
    }
}