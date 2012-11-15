using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Spatial;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace Xco.Models
{


    public class Link
    {
        public int LinkId { get; set; }

        [DataType(DataType.Url)]
        public string ShortenedUrl { get; set; }
        
        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "URL to shorten")]
        public string OriginalUrl { get; set; }
        
        public DateTime CreatedOn { get; set; }
        public string CreatedByEmail { get; set; }
        
        public bool IsVanityUrl { get; set; }

        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
        
        public  ICollection<Visit> Visits{ get; set; }

   }

    public class Visit
    {
        public int VisitId { get; set; }
        
        public int LinkId { get; set; }
        public Link Link { get; set; }

        public DateTime Stamp { get; set; }
        public DbGeography Location { get; set; }

    }

    /// <summary>
    /// Non-EF class that creates strings
    /// </summary>
    public static class UrlShortener
    {

        /// <summary>
        ///     Generate a semi-random short string of a specific length, having only characters in the @alphabet
        /// </summary>
        /// <param name="ReturnStringLength">Length of string to return</param>
        /// <param name="Alphabet">String containing charaters to consider</param>
        /// <returns>A random string of length @ReturnStringLength.  Relatively rarely repetitive</returns>
        public static string Generate(int ReturnStringLength = 7, string Alphabet = "bcdfghjkmnpqrstvwxyzBCDFGHJKLMNPQRSTVWXYZ23456789")
        {
            if(ReturnStringLength <= 0) return "";

            StringBuilder sb = new StringBuilder();
            int remainder;
            long seed = DateTime.Now.Ticks;
            while (sb.Length < ReturnStringLength)
            {
                remainder = (int)(seed % Alphabet.Length);
                seed = Math.Abs(seed * (long) Math.Pow(remainder + 1, 2));

                sb.Append(Alphabet[remainder]);
            };

            return sb.ToString();
        }

    }

}