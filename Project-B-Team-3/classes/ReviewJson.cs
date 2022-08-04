using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectB
{
    public class Review
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Rating")]
        public int Rating { get; set; }

        [JsonPropertyName("Text")]
        public string Text { get; set; }
    }
    internal class ReviewsList
    {
        public List<Review> _reviews;
        private string ReviewJsonName = @$"Data{Path.DirectorySeparatorChar}Reviews.json";
        
        public ReviewsList()
        {
            if (File.Exists(ReviewJsonName))
            {
                    try
                    {
                        Load();
                    }
                    catch
                    {
                        _reviews = new List<Review>();
                    }
            }
            else
            {
                File.Create(ReviewJsonName);
                _reviews = new List<Review>();
            }
        }

        public void Add(string text, int rating)
        {
            Review add_Review = new Review();
            add_Review.Name = Program.information.Member.Firstname;
            add_Review.Rating = rating;
            add_Review.Text = text;
            
            _reviews.Add(add_Review);
            Save();
        }

        public int Remove(int index)
        {
            Load();
            for (int i = 0; i < _reviews.Count; i++)
            {
                if (i == index)
                {
                    _reviews.RemoveAt(i);
                    Save();
                    return i;
                }
            }
            Save();
            return -1;

        }

        public void Save()
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            File.WriteAllText(ReviewJsonName, JsonSerializer.Serialize(_reviews, options: options));
        }

        public void Load()
        {
            string json = File.ReadAllText(ReviewJsonName);
            _reviews = JsonSerializer.Deserialize<List<Review>>(json);
        }
    }
}
