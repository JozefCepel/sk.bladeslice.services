namespace WebEas.Esam.Reports
{
    public interface IReportInfo
    {
        /// <summary>
        /// Gets or sets the document author.
        /// </summary>
        /// <value>The document author.</value>
        string DocumentAuthor { get; set; }

        /// <summary>
        /// Gets or sets the document subject.
        /// </summary>
        /// <value>The document subject.</value>
        string DocumentSubject { get; set; }

        /// <summary>
        /// Gets or sets the document keywords.
        /// </summary>
        /// <value>The document keywords.</value>
        string DocumentKeywords { get; set; }

        /// <summary>
        /// Gets or sets the document title.
        /// </summary>
        /// <value>The document title.</value>
        string DocumentTitle { get; set; }
    }
}
