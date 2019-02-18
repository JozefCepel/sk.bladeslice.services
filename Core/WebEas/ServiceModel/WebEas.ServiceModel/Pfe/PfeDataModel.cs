using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract(Name = "DataModel")]
    public class PfeDataModel : IEquatable<PfeDataModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PfeDataModel" /> class.
        /// </summary>
        public PfeDataModel()
        {
            this.Type = PfeModelType.None;
            this.ShowInActions = false;
            this.RowFilterEnabled = false;
            this.SummaryRowEnabled = false;
            this.SummaryRow = new List<PfeSummaryRow>();
            this.MultiSortEnabled = false;
            this.RequiresFilter = false;
            this.Fields = new List<PfeColumnAttribute>();
            this.FieldGroups = new List<PfeGroupAttribute>();
            this.MultiSort = new List<PfeSortAttribute>();
            this.Filters = new List<PfeFilterAttribute>();
            this.Pages = new List<PfePage>();
            this.Layout = new List<PfeLayout>();
            this.SelectionMode = PfeSelection.Single;
        }

        /// <summary>
        /// Datovy model pre pohlad
        /// </summary>
        /// <param name="name">Nazov pohladu</param>
        /// <param name="type">Typ pohladu</param>
        /// <param name="rowFilter">Povolenie filtrovania riadkov</param>
        /// <param name="multiSort">Povolenie usporiadania stlpcov</param>
        public PfeDataModel(string name, PfeModelType type, bool showinactions = false, bool rowFilter = true, bool multiSort = true) : this()
        {
            this.Name = name;
            this.Type = type;
            this.ShowInActions = showinactions;
            this.RowFilterEnabled = rowFilter;
            this.MultiSortEnabled = multiSort;
        }

        /// <summary>
        /// Id pohladu
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// Nazov pohladu
        /// </summary>
        [DataMember(Name = "nam")]
        public string Name { get; set; }

        /// <summary>
        /// Typ pohladu
        /// </summary>
        [DataMember(Name = "typ")]
        public PfeModelType Type { get; set; }

        /// <summary>
        /// Vyžadovanie filtra
        /// </summary>
        [DataMember(Name = "rfl")]
        public bool RequiresFilter { get; set; }

        /// <summary>
        /// Čakať na vstupné údaje
        /// </summary>
        [DataMember(Name = "wfid")]
        public bool? WaitForInputData { get; set; }

        /// <summary>
        /// Zobrazenie pohlad v dostupnych akciach
        /// </summary>
        [DataMember(Name = "sia")]
        public bool ShowInActions { get; set; }

        /// <summary>
        /// Cislo master pohladu
        /// </summary>
        [DataMember(Name = "mvw")]
        public int? MasterView { get; set; }

        /// <summary>
        /// Povolenie filtrovanie riadkov
        /// </summary>
        [DataMember(Name = "rfe")]
        public bool RowFilterEnabled { get; set; }

        /// <summary>
        /// Povolenie usporiadania stlpcov
        /// </summary>
        [DataMember(Name = "mse")]
        public bool MultiSortEnabled { get; set; }

        /// <summary>
        /// Zoznam stlpcov pohladu
        /// </summary>
        [DataMember(Name = "flds")]       
        public List<PfeColumnAttribute> Fields { get; set; }

        /// <summary>
        /// Zoznam skupin pohladu
        /// </summary>
        [DataMember(Name = "flgs")]  
        public List<PfeGroupAttribute> FieldGroups { get; set; }

        /// <summary>
        /// Zoznam stlpcov usporadania
        /// </summary>
        [DataMember(Name = "srts")]  
        public List<PfeSortAttribute> MultiSort { get; set; }

        /// <summary>
        /// Filtrovanie dat
        /// </summary>
        [DataMember(Name = "flts")]         
        public List<PfeFilterAttribute> Filters { get; set; }

        /// <summary>
        /// Data pre typ pohladu Form
        /// </summary>        
        [DataMember(Name = "pges")]         
        public List<PfePage> Pages { get; set; }

        /// <summary>
        /// Data pre typ pohladu Layout
        /// </summary>
        [DataMember(Name= "lyts")]
        public List<PfeLayout> Layout { get; set; }

        /// <summary>
        /// Data pre typ selection mode.
        /// </summary>
        /// <value>The selection mode.</value>
        [DataMember(Name = "sel")]
        public PfeSelection SelectionMode { get; set; }

        /// <summary>
        /// Data pre typ double click action.
        /// </summary>
        /// <value>The double click action.</value>
        [DataMember(Name = "dca")]
        public NodeAction DoubleClickAction { get; set; }

        /// <summary>
        /// Summary row enabled
        /// </summary>
        [DataMember(Name = "sre")]
        public bool SummaryRowEnabled { get; set; }

        /// <summary>
        /// Summary row
        /// </summary>
        [DataMember(Name = "srow")]
        public List<PfeSummaryRow> SummaryRow { get; set; }

        /// <summary>
        /// Shoulds the serialize actions.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRowFilterEnabled()
        {
            return (this.Type != PfeModelType.Form || this.Type != PfeModelType.Layout);
        }

        /// <summary>
        /// Shoulds the serialize id.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeId()
        {
            return Id!=null && Id != 0;
        }

        /// <summary>
        /// Shoulds the serialize actions.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeMultiSortEnabled()
        {
            return (this.Type != PfeModelType.Form || this.Type != PfeModelType.Layout);
        }

        /// <summary>
        /// Shoulds the serialize actions.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeFieldGroups()
        {
            return (this.Type != PfeModelType.Form || this.Type != PfeModelType.Layout);
        }

        /// <summary>
        /// Shoulds the serialize actions.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeMultiSort()
        {
            return (this.Type != PfeModelType.Form || this.Type != PfeModelType.Layout);
        }

        /// <summary>
        /// Shoulds the serialize actions.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeFilters()
        {
            return (this.Type != PfeModelType.Form || this.Type != PfeModelType.Layout);
        }

        /// <summary>
        /// Shoulds the serialize actions.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializePages()
        {
            return this.Type == PfeModelType.Form;
        }

        /// <summary>
        /// Shoulds the serialize actions.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeLayout()
        {
            return this.Type == PfeModelType.Layout;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, Type: {2}, ShowInActions: {3}, MasterView: {4}, RowFilterEnabled: {5}, MultiSortEnabled: {6}, Fields: {7}, FieldGroups: {8}, MultiSort: {9}, Filters: {10}, Pages: {11}, Layout: {12}, SelectionMode: {13}, RequiresFilter: {14}, DoubleClickAction: {15}, WaitForInputData: {16}, SummaryRowEnabled: {17}, SummaryRow: {18}", this.Id, this.Name, this.Type, this.ShowInActions, this.MasterView, this.RowFilterEnabled, this.MultiSortEnabled, this.Fields, this.FieldGroups, this.MultiSort, this.Filters, this.Pages, this.Layout, this.SelectionMode, this.RequiresFilter, this.DoubleClickAction, this.WaitForInputData, SummaryRowEnabled, SummaryRow);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + this.Id.GetHashCode();
                result = result * 23 + ((this.Name != null) ? this.Name.GetHashCode() : 0);
                result = result * 23 + this.Type.GetHashCode();
                result = result * 23 + this.ShowInActions.GetHashCode();
                result = result * 23 + this.MasterView.GetHashCode();
                result = result * 23 + this.RowFilterEnabled.GetHashCode();
                result = result * 23 + this.SummaryRowEnabled.GetHashCode();
                result = result * 23 + ((this.SummaryRow != null) ? this.SummaryRow.GetHashCode() : 0);
                result = result * 23 + this.MultiSortEnabled.GetHashCode();
                result = result * 23 + this.RequiresFilter.GetHashCode();
                result = result * 23 + ((this.WaitForInputData != null) ? this.WaitForInputData.GetHashCode() : 0);
                result = result * 23 + ((this.Fields != null) ? this.Fields.GetHashCode() : 0);
                result = result * 23 + ((this.FieldGroups != null) ? this.FieldGroups.GetHashCode() : 0);
                result = result * 23 + ((this.MultiSort != null) ? this.MultiSort.GetHashCode() : 0);
                result = result * 23 + ((this.Filters != null) ? this.Filters.GetHashCode() : 0);
                result = result * 23 + ((this.Pages != null) ? this.Pages.GetHashCode() : 0);
                result = result * 23 + ((this.Layout != null) ? this.Layout.GetHashCode() : 0);
                result = result * 23 + this.SelectionMode.GetHashCode();
                result = result * 23 + ((this.DoubleClickAction != null) ? this.DoubleClickAction.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(PfeDataModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return this.Id == other.Id &&
                   Equals(this.Name, other.Name) &&
                   this.Type.Equals(other.Type) &&
                   this.ShowInActions.Equals(other.ShowInActions) &&
                   this.MasterView.Equals(other.MasterView) &&
                   this.RowFilterEnabled == other.RowFilterEnabled &&
                   this.SummaryRowEnabled == other.SummaryRowEnabled &&
                   Equals(this.SummaryRow, other.SummaryRow) &&
                   this.MultiSortEnabled == other.MultiSortEnabled &&
                   this.RequiresFilter == other.RequiresFilter &&
                   Equals(this.WaitForInputData, other.WaitForInputData) &&
                   Equals(this.Fields, other.Fields) &&
                   Equals(this.FieldGroups, other.FieldGroups) &&
                   Equals(this.MultiSort, other.MultiSort) &&
                   Equals(this.Filters, other.Filters) &&
                   Equals(this.Pages, other.Pages) &&
                   Equals(this.Layout, other.Layout) &&
                   Equals(this.SelectionMode, other.Layout) &&
                   Equals(this.DoubleClickAction, other.DoubleClickAction);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise,
        /// false.
        /// </returns>
        public override bool Equals(object obj)
        {
            PfeDataModel temp = obj as PfeDataModel;
            if (temp == null)
            {
                return false;
            }
            return this.Equals(temp);
        }
    }
}
