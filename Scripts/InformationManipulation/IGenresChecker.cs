namespace dockerapi.Scripts.InformationManipulation
{
    public interface IGenresChecker
    {
        /// <summary>
        /// Add an item to the list we want.
        /// </summary>
        /// <param name="item">An item represented in a type of a String. We expect this to be a music genre type.</param>
        void AddItem(string item);

        /// <summary>
        /// Remove all of the items from the default item list.
        /// </summary>
        public void ClearItemsList();

        /// <summary>
        /// Check if an item specified by the user is present in the genres defined list.
        /// </summary>
        /// <param name="item">Item you want to check for either being present in the list or not.</param>
        /// <returns>Boolean value indicating True or False for an item being in the list or not.</returns>
        bool CheckItem(string item);

        /// <summary>
        /// Check whether the item exists and return the position of it. If it does not exist, -1 will be returned.
        /// </summary>
        /// <param name="item">The String value we want to check - if it's present in our list.</param>
        /// <returns>an integer value indicating in what position is the item that has been found, unless it doesn't
        /// exist, and then -1 will be returned.</returns>
        int CheckItemPosition(string item);
    }
}