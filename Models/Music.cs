namespace dockerapi.Models
{
    public class Music
    {
        /// <summary>
        /// Name of the Band.
        /// </summary>
        public string Band { get; set; }

        /// <summary>
        /// Name of the Song performed by a particular band.
        /// </summary>
        public string Song { get; set; }

        /// <summary>
        /// Style towards which a specific song is related.
        /// </summary>
        public string Style { get; set; }
    }
}