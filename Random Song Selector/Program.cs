using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

// API URL
string apiUrl = "https://itunes.apple.com/search?term=rock&entity=song";

// main entry point
await MainAsync();

async Task MainAsync()
{
    bool shouldContinue = true;

    while (shouldContinue)
    {
        await FetchRandomSongDetails(apiUrl);

        Console.WriteLine("Press any key to fetch another song or 'q' to quit...");
        if (Console.ReadKey().Key == ConsoleKey.Q)
        {
            shouldContinue = false;
        }
        Console.Clear(); 
    }
}

async Task FetchRandomSongDetails(string url)
{
    using HttpClient client = new HttpClient();
    HttpResponseMessage response = await client.GetAsync(url);
    response.EnsureSuccessStatusCode();
    string responseBody = await response.Content.ReadAsStringAsync();

    JObject json = JObject.Parse(responseBody);
    var songs = json["results"];
    if (songs.HasValues)
    {
        Random random = new Random();
        int index = random.Next(songs.Count());

        // Extracting song details
        string songTitle = songs[index]["trackName"]?.ToString();
        string artistName = songs[index]["artistName"]?.ToString();
        string genre = songs[index]["primaryGenreName"]?.ToString();
        string playtime = songs[index]["trackTimeMillis"] != null
            ? (Convert.ToInt32(songs[index]["trackTimeMillis"]) / 1000).ToString() 
            : "Unknown";

        
        int totalSeconds = int.Parse(playtime);
        string formattedPlaytime = $"{totalSeconds / 60}m {totalSeconds % 60}s";

       
        Console.WriteLine("Random Song: " + songTitle);
        Console.WriteLine("Artist: " + artistName);
        Console.WriteLine("Genre: " + genre);
        Console.WriteLine("Playtime: " + formattedPlaytime);
    }
    else
    {
        Console.WriteLine("No songs found.");
    }
}
