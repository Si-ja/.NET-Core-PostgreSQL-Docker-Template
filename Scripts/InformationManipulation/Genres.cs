using System;
using System.Collections.Generic;

namespace dockerapi.Scripts.InformationManipulation
{
    public class Genres 
    {
        private List<String> genres;
        public Genres()
        {
            this.genres = new List<String>();
        }

        /// <summary>
        /// Add an item to the list we want.
        /// </summary>
        /// <param name="item">An item represented in a type of a String. We expect this to be a music genre type.</param>
        public void AddItem(String item)
        {
            // Lower the String text of how our items are written, to know for sure that neither us or the user will spell something 
            // With the incorrect formation and that will be responsible for not finding something in the genres when they exist
            this.genres.Add(item.ToLower());
        }

        /// <summary>
        /// Check if an item specified by the user is present in the genres defined list.
        /// </summary>
        /// <param name="item">Item you want to check for either being present in the list or not.</param>
        /// <returns>Boolean value indicating True or False for an item being in the list or not.</returns>
        private bool CheckItem(String item)
        {
            bool status;
            // Remember to lower our text down.
            status = this.genres.Contains(item: item.ToLower());

            return status;
        }

        /// <summary>
        /// Check whether the item exists and return the position of it. If it does not exist, -1 will be returned.
        /// </summary>
        /// <param name="item">The String value we want to check - if it's present in our list.</param>
        /// <returns>an integer value indicating in what position is the item that has been found, unless it doesn't
        /// exist, and then -1 will be returned.</returns>
        public int CheckItemPosition(String item)
        {
            // First check if the item even exists
            bool status;
            int answer = -1;
            status = this.CheckItem(item: item.ToLower());
            if (!status)
            {
                return answer;
            } 
            else 
            {
                // Otherwise we need to search on
                for (int i=0; i<this.genres.Count; i++)
                {
                    answer = i + 1;
                    if (this.genres[i] == item.ToLower())
                    {
                        return answer;
                    }
                }
            }

            // If what not - we need a path to return an error
            return answer;
        }
    }
}