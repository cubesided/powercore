using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Powercore
{
    public class UrlChecker
    {
        public static async Task<bool> DoesUrlExist(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Send a HEAD request to avoid downloading the full content,
                    // which is more efficient for simply checking existence.
                    HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

                    // Check for a successful status code (e.g., 200 OK, 3xx Redirect)
                    // A 404 Not Found indicates the URL does not exist.
                    return response.IsSuccessStatusCode;
                }
            }
            catch (HttpRequestException)
            {
                // Handle network errors, DNS resolution issues, etc.
                return false;
            }
            catch (Exception ex)
            {
                // Handle other potential exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static async Task Main(string[] args)
        {
            string existingUrl = "https://www.google.com";
            string nonExistentUrl = "https://www.example.com/nonexistentpage123";

            if (await DoesUrlExist(existingUrl))
            {
                Console.WriteLine($"The URL '{existingUrl}' exists.");
            }
            else
            {
                Console.WriteLine($"The URL '{existingUrl}' does not exist.");
            }

            if (await DoesUrlExist(nonExistentUrl))
            {
                Console.WriteLine($"The URL '{nonExistentUrl}' exists.");
            }
            else
            {
                Console.WriteLine($"The URL '{nonExistentUrl}' does not exist.");
            }
        }

    }
}