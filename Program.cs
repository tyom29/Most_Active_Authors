using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft;
using Newtonsoft.Json;


class JsonParent
{
    public string page { get; set; }
    public string per_page { get; set; }

    public string total { get; set; }
    public int total_pages { get; set; }
    public List<DataCh> data { get; set; }

}

class DataCh    
{
    public int id { get; set; }
    public string username { get; set; }
    public string about { get; set; }
    public string submitted { get; set; }
    public string updated_at { get; set; }
    public int submission_count { get; set; }
    public string comment_count { get; set; }
    public string created_at { get; set; }
}


class Result
{

    public static List<string> getUsernames(int threshold)
    {
        List<string> usernames = new List<string>();
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage responseMessage = client.GetAsync("https://jsonmock.hackerrank.com/api/article_users?page=").Result;
            
            var responseJson = JsonConvert.DeserializeObject<JsonParent>(responseMessage.Content.ReadAsStringAsync().Result);
            for (int i = 0; i <= responseJson.total_pages; i++)
            {
                var page_response = client.GetAsync("https://jsonmock.hackerrank.com/api/article_users/search?page=" + i).Result;
                var page_content = JsonConvert.DeserializeObject<JsonParent> (page_response.Content.ReadAsStringAsync().Result);

                for (int j = 0; j < page_content.data.Count; j++)
                {
                    if (Convert.ToInt32(page_content.data[j].submission_count) > threshold)
                    {
                        if (usernames.Count< threshold) // for adding usernames by threshold size.  
                        {
                            usernames.Add((Convert.ToString(page_content.data[j].username)));
                        }
                    }
                }
            }
        }
        return usernames;
    }
}
class Solution
{
    public static void Main(string[] args)
    {

        int threshold = Convert.ToInt32(Console.ReadLine().Trim());

        List<string> result = Result.getUsernames(threshold);

        Console.WriteLine("Threshold = "+ threshold);
        Console.WriteLine("UsernamesCount = " + result.Count);
        foreach (var item in result)
        {
            Console.WriteLine(item);
        }

        Console.ReadLine();
    }
}
