using System.Text;
using System.Threading.Channels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main()
    {
        await Console.Out.WriteLineAsync("v0.1");
        await Console.Out.WriteLineAsync("Warning this app is on Early Access follow https://github.com/Roalexx for new versions");

        await Console.Out.WriteLineAsync("first go to your profile in website then press ctrl+shift+c then go to network tab and press following then right click on following/?count=12 press copy and copy as curl(bash) and paste this curl here");

        string curlCommand = ReadMultilineInput();

        string command = curlCommand.ToString(); // we took curl as string

        string cookie = cookieFinder(command); // we have cookie

        string xIgAppId = xIgAppIdFinder(command); // we have x-ig-app-id

        Console.Clear();

        await Console.Out.WriteLineAsync("Enter the username you want to perform the operation with. Note: If you are not going to perform the operation with your own username, make sure the account you enter is followed by the account using the curl command.");

        string userChecker = "";

        string userName = "";

        while (userChecker == "")
        {
            userChecker = Console.ReadLine();
            if (userChecker != "")
            {
                await Console.Out.WriteLineAsync("sizin yada kontrol etmek istediginiz hesabibn adini giriniz");
                userName = userChecker;
            }
            else
            {
                await Console.Out.WriteLineAsync("Lutfen yanit giriniz");
            }
        }// kullanici adi aldik

        Console.Clear();

        string userLink = "https://www.instagram.com/api/v1/users/web_profile_info/?username=" + userName;

        JObject userData = await AsyncUserDataRequest(userLink, xIgAppId);

        string userId = (string)((Newtonsoft.Json.Linq.JValue)userData["data"]["user"]["id"]).Value; //We have userId for more requests

        long userFollowerCount = (long)((Newtonsoft.Json.Linq.JValue)userData["data"]["user"]["edge_followed_by"]["count"]).Value;

        long userFollowingCount = (long)((Newtonsoft.Json.Linq.JValue)userData["data"]["user"]["edge_follow"]["count"]).Value;  // we need users follower and follwing count so we have it here

        JObject followingData = await AsyncGetUserFollowing(xIgAppId, userId, cookie, userFollowingCount); //now we have the following users

        InstagramResponse following = JsonConvert.DeserializeObject<InstagramResponse>(followingData.ToString()); // we need to use json datas so we convert it to a usable form

        JObject followersData = await AsyncGetUserFollowers(xIgAppId, userId, cookie, userFollowerCount); // now we have followers

        InstagramResponse followers = JsonConvert.DeserializeObject<InstagramResponse>(followersData.ToString());

        List<string> comparedFollowers = CompareFollows(following, followers); //the followers didnt follow you back

        await Console.Out.WriteLineAsync("followers did't follow you back -------------->");

        foreach (var users in comparedFollowers)
        {
            await Console.Out.WriteLineAsync(users);
        }

        await Console.Out.WriteLineAsync("----------------------------------------------");

        List<string> comparedFollowing = CompareFollows(followers, followers); //the followers didnt you follow

        await Console.Out.WriteLineAsync("followers didn't you follow ----------------->");

        foreach (var users in comparedFollowing)
        {
            await Console.Out.WriteLineAsync(users);
        }

        await Console.Out.WriteLineAsync("----------------------------------------------");

    }

    static List<string> CompareFollows(InstagramResponse main, InstagramResponse compared)
    {
        HashSet<string> order = new HashSet<string>();
        HashSet<string> other = new HashSet<string>();

        foreach (var user in main.Users) { order.Add(user.Username); }

        foreach (var user in compared.Users) { other.Add(user.Username); }

        List<string> result = new List<string>();

        foreach (var username in order)
        {
            if (!other.Contains(username))
            {
                result.Add(username);
            }
        }

        return result;
    }

    static async Task<JObject> AsyncGetUserFollowing(string xIgAppId, string userId, string cookie, long userFollowingCount)
    {
        string getFollowingLink = "https://www.instagram.com/api/v1/friendships/" + userId + "/following/?count=200&max_id=";
        string url;
        JObject result = new JObject();

        if (userFollowingCount > 1200)
        {
            await Console.Out.WriteLineAsync("you cant use the app  more than 1200 following");
        }
        else
        {
            long counter = userFollowingCount / 200;
            long mod = userFollowingCount % 200;

            for (int i = 0; i < counter; i++)
            {
                url = getFollowingLink + (i * 200);
                JObject following = await AsyncHttpRequest(url, cookie, xIgAppId);
                result.Merge(following);
            }

            url = getFollowingLink + (userFollowingCount - mod);
            JObject restFollowing = await AsyncHttpRequest(url, cookie, xIgAppId);
            result.Merge(restFollowing);

        }

        return result;
    }

    static async Task<JObject> AsyncGetUserFollowers(string xIgAppId, string userId, string cookie, long userFollowerCount)
    {

        string getFollowerLink = "https://www.instagram.com/api/v1/friendships/" + userId + "/followers/?count=12&max_id=";
        string url;
        string nextMaxId = "";
        JObject result = new JObject();

        if (userFollowerCount > 256)
        {
            await Console.Out.WriteLineAsync("you cant use the app  more than 256 followers");
        }
        else
        {
            int counter = Convert.ToInt32(userFollowerCount) / 12;
            if ((userFollowerCount / 12) % 12 != 0) counter++;

            for (int i = 0; i < counter; i++)
            {
                url = getFollowerLink + nextMaxId;
                JObject followers = await AsyncHttpRequest(url, cookie, xIgAppId);
                result.Merge(followers);
                InstagramResponse forNextMaxId = JsonConvert.DeserializeObject<InstagramResponse>(followers.ToString());
                nextMaxId = forNextMaxId.NextMaxId;
            }
        }

        return result;
    }

    static async Task<JObject> AsyncUserDataRequest(string userLink, string xIgAppId)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("x-ig-app-id", xIgAppId);

            HttpResponseMessage response = await client.GetAsync(userLink);
            string content = await response.Content.ReadAsStringAsync();
            JObject jsonObject = JObject.Parse(content);

            return jsonObject;
        }

    }

    static async Task<JObject> AsyncHttpRequest(string userLink, string cookie, string xIgAppId)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("cookie", cookie);
            client.DefaultRequestHeaders.Add("x-ig-app-id", xIgAppId);

            HttpResponseMessage response = await client.GetAsync(userLink);
            JObject jsonObject;
            string content = await response.Content.ReadAsStringAsync();
            jsonObject = JObject.Parse(content);
            return jsonObject;
        }
    }

    static string ReadMultilineInput()
    {
        string input;
        string result = string.Empty;

        while (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
        {
            result += input + " ";
        }
        return result;
    }

    static string cookieFinder(string command)
    {
        int startIndex = command.IndexOf("cookie: ");

        if (startIndex == -1)
        {
            return null;
        }

        startIndex += "cookie".Length;

        string endSymbol = "-H \"referer:";
        int endIndex = command.IndexOf(endSymbol, startIndex);

        if (endIndex == -1)
        {
            return null;
        }

        string cookieValue = command.Substring(startIndex + 2, endIndex - startIndex - 9).Trim();

        cookieValue = cookieValue.Replace("^\\^", "");

        cookieValue = cookieValue.Replace("^%^", "%");

        cookieValue = cookieValue.Replace("^", "\\");

        return cookieValue;
    }

    static string xIgAppIdFinder(string command)
    {
        int startIndex = command.IndexOf("x-ig-app-id:");

        if (startIndex == -1)
        {
            return null;
        }

        startIndex += "x-ig-app-id:".Length;

        int endIndex = command.IndexOf("\"", startIndex);

        if (endIndex == -1)
        {
            return null;
        }

        string xIgAppId = command.Substring(startIndex, endIndex - startIndex).Trim();

        return xIgAppId;
    }

}