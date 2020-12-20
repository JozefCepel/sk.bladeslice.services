using System;
using System.Collections.Generic;
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
            Type = PfeModelType.None;
            ShowInActions = false;
            RowFilterEnabled = false;
            SummaryRowEnabled = false;
            SummaryRow = new List<PfeSummaryRow>();
            MultiSortEnabled = false;
            RequiresFilter = false;
            Fields = new List<PfeColumnAttribute>();
            FieldGroups = new List<PfeGroupAttribute>();
            MultiSort = new List<PfeSortAttribute>();
            Filters = new List<PfeFilterAttribute>();
            Pages = new List<PfePage>();
            Layout = new List<PfeLayout>();
            SelectionMode = PfeSelection.Single;
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
            Name = name;
            Type = type;
            ShowInActions = showinactions;
            RowFilterEnabled = rowFilter;
            MultiSortEnabled = multiSort;
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

        [DataMember(Name = "uab")]
        public bool? UseAsBrowser { get; set; }

        [DataMember(Name = "uabr")]
        public int? UseAsBrowserRank { get; set; }

        [DataMember(Name = "pivt")]
        public PfePivot Pivot { get; set; }

        /// <summary>
        /// Shoulds the serialize actions.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRowFilterEnabled()
        {
            return (Type != PfeModelType.Form || Type != PfeModelType.Layout);
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
            return (Type != PfeModelType.Form || Type != PfeModelType.Layout);
        }

        /// <summary>
        /// Shoulds the serialize FieldGroups.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeFieldGroups()
        {
            return (Type != PfeModelType.Form || Type != PfeModelType.Layout); // && FieldGroups.Count > 0
        }

        /// <summary>
        /// Shoulds the serialize MultiSort.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeMultiSort()
        {
            return (Type != PfeModelType.Form || Type != PfeModelType.Layout); // && MultiSort.Count > 0
        }

        /// <summary>
        /// Shoulds the serialize Filters.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeFilters()
        {
            return (Type != PfeModelType.Form || Type != PfeModelType.Layout); // && Filters.Count > 0;
        }

        //Padá na JS chybe
        /// <summary>
        /// Shoulds the serialize SummaryRow.
        /// </summary>
        /// <returns></returns>
        //public bool ShouldSerializeSummaryRow()
        //{
        //    return (Type != PfeModelType.Form || Type != PfeModelType.Layout) && SummaryRow.Count > 0;
        //}


        /// <summary>
        /// Shoulds the serialize Pages.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializePages()
        {
            return Type == PfeModelType.Form;
        }

        /// <summary>
        /// Shoulds the serialize Layout.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeLayout()
        {
            return Type == PfeModelType.Layout;
        }

        /// <summary>
        /// Shoulds the serialize pivot.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializePivot()
        {
            return Pivot != null;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Id: {Id}," +
                $" Name: {Name}," +
                $" Type: {Type}," +
                $" ShowInActions: {ShowInActions}," +
                $" RowFilterEnabled: {RowFilterEnabled}," +
                $" MultiSortEnabled: {MultiSortEnabled}," +
                $" Fields: {Fields}," +
                $" FieldGroups: {FieldGroups}," +
                $" MultiSort: {MultiSort}," +
                $" Filters: {Filters}," +
                $" Pages: {Pages}," +
                $" Layout: {Layout}," +
                $" SelectionMode: {SelectionMode}," +
                $" RequiresFilter: {RequiresFilter}," +
                $" DoubleClickAction: {DoubleClickAction}," +
                $" WaitForInputData: {WaitForInputData}," +
                $" SummaryRowEnabled: {SummaryRowEnabled}," +
                $" SummaryRow: {SummaryRow}," +
                $" UseAsBrowser: {UseAsBrowser}," +
                $" UseAsBrowserRank: {UseAsBrowserRank}" +
                $" Pivot: {Pivot}";
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
                result = result * 23 + Id.GetHashCode();
                result = result * 23 + ((Name != null) ? Name.GetHashCode() : 0);
                result = result * 23 + Type.GetHashCode();
                result = result * 23 + ShowInActions.GetHashCode();
                result = result * 23 + RowFilterEnabled.GetHashCode();
                result = result * 23 + SummaryRowEnabled.GetHashCode();
                result = result * 23 + ((SummaryRow != null) ? SummaryRow.GetHashCode() : 0);
                result = result * 23 + MultiSortEnabled.GetHashCode();
                result = result * 23 + RequiresFilter.GetHashCode();
                result = result * 23 + ((WaitForInputData != null) ? WaitForInputData.GetHashCode() : 0);
                result = result * 23 + ((Fields != null) ? Fields.GetHashCode() : 0);
                result = result * 23 + ((FieldGroups != null) ? FieldGroups.GetHashCode() : 0);
                result = result * 23 + ((MultiSort != null) ? MultiSort.GetHashCode() : 0);
                result = result * 23 + ((Filters != null) ? Filters.GetHashCode() : 0);
                result = result * 23 + ((Pages != null) ? Pages.GetHashCode() : 0);
                result = result * 23 + ((Layout != null) ? Layout.GetHashCode() : 0);
                result = result * 23 + SelectionMode.GetHashCode();
                result = result * 23 + ((DoubleClickAction != null) ? DoubleClickAction.GetHashCode() : 0);
                result = result * 23 + ((UseAsBrowser != null) ? UseAsBrowser.GetHashCode() : 0);
                result = result * 23 + ((UseAsBrowserRank != null) ? UseAsBrowserRank.GetHashCode() : 0);
                result = result * 23 + ((Pivot != null) ? Pivot.GetHashCode() : 0);
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
            return Id == other.Id &&
                   Equals(Name, other.Name) &&
                   Type.Equals(other.Type) &&
                   ShowInActions.Equals(other.ShowInActions) &&
                   RowFilterEnabled == other.RowFilterEnabled &&
                   SummaryRowEnabled == other.SummaryRowEnabled &&
                   Equals(SummaryRow, other.SummaryRow) &&
                   MultiSortEnabled == other.MultiSortEnabled &&
                   RequiresFilter == other.RequiresFilter &&
                   Equals(WaitForInputData, other.WaitForInputData) &&
                   Equals(Fields, other.Fields) &&
                   Equals(FieldGroups, other.FieldGroups) &&
                   Equals(MultiSort, other.MultiSort) &&
                   Equals(Filters, other.Filters) &&
                   Equals(Pages, other.Pages) &&
                   Equals(Layout, other.Layout) &&
                   Equals(SelectionMode, other.SelectionMode) &&
                   Equals(DoubleClickAction, other.DoubleClickAction) &&
                   Equals(UseAsBrowser, other.UseAsBrowser) &&
                   Equals(UseAsBrowserRank, other.UseAsBrowserRank) &&
                   Equals(Pivot, other.Pivot);
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
            return Equals(temp);
        }
    }
}
