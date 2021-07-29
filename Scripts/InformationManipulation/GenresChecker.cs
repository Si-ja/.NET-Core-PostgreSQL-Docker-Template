using System;
using System.Collections.Generic;

namespace dockerapi.Scripts.InformationManipulation
{
    public class GenresChecker : IGenresChecker
    {
        private readonly List<String> genres;
        public GenresChecker()
        {
            this.genres = new List<String>();
        }

        public void ClearItemsList()
        {
            this.genres.Clear();
        }

        public void AddItem(String item)
        {
            this.genres.Add(item.ToLower());
        }

        public bool CheckItem(String item)
        {
            bool status;
            status = this.genres.Contains(item: item.ToLower());

            return status;
        }

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
                for (int i = 0; i < this.genres.Count; i++)
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