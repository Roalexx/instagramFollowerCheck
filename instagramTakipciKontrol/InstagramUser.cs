using Newtonsoft.Json;

public class InstagramUser
{
    [JsonProperty("fbid_v2")]
    public string FbidV2 { get; set; }

    [JsonProperty("pk")]
    public string PK { get; set; }

    [JsonProperty("pk_id")]
    public string PKId { get; set; }

    [JsonProperty("strong_id__")]
    public string StrongId { get; set; }

    [JsonProperty("full_name")]
    public string FullName { get; set; }

    [JsonProperty("is_private")]
    public bool IsPrivate { get; set; }

    [JsonProperty("third_party_downloads_enabled")]
    public int ThirdPartyDownloadsEnabled { get; set; }

    [JsonProperty("has_anonymous_profile_picture")]
    public bool HasAnonymousProfilePicture { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("is_verified")]
    public bool IsVerified { get; set; }

    [JsonProperty("profile_pic_id")]
    public string ProfilePicId { get; set; }

    [JsonProperty("profile_pic_url")]
    public string ProfilePicUrl { get; set; }

    [JsonProperty("account_badges")]
    public List<object> AccountBadges { get; set; }

    [JsonProperty("is_possible_scammer")]
    public bool IsPossibleScammer { get; set; }

    [JsonProperty("is_possible_bad_actor")]
    public IsPossibleBadActor IsPossibleBadActor { get; set; }

    [JsonProperty("latest_reel_media")]
    public int LatestReelMedia { get; set; }

    [JsonProperty("is_favorite")]
    public bool IsFavorite { get; set; }
}

public class IsPossibleBadActor
{
    [JsonProperty("is_possible_scammer")]
    public bool IsPossibleScammer { get; set; }

    [JsonProperty("is_possible_impersonator")]
    public IsPossibleImpersonator IsPossibleImpersonator { get; set; }

    [JsonProperty("is_possible_impersonator_threads")]
    public IsPossibleImpersonatorThreads IsPossibleImpersonatorThreads { get; set; }
}

public class IsPossibleImpersonator
{
    [JsonProperty("is_unconnected_impersonator")]
    public bool IsUnconnectedImpersonator { get; set; }

    [JsonProperty("connected_similar_user_id")]
    public object ConnectedSimilarUserId { get; set; }
}

public class IsPossibleImpersonatorThreads
{
    [JsonProperty("is_unconnected_impersonator")]
    public bool IsUnconnectedImpersonator { get; set; }

    [JsonProperty("connected_similar_user_id")]
    public object ConnectedSimilarUserId { get; set; }
}

public class InstagramResponse
{
    [JsonProperty("users")]
    public List<InstagramUser> Users { get; set; }

    [JsonProperty("big_list")]
    public bool BigList { get; set; }

    [JsonProperty("page_size")]
    public int PageSize { get; set; }

    [JsonProperty("next_max_id")]
    public string NextMaxId { get; set; }

    [JsonProperty("has_more")]
    public bool HasMore { get; set; }

    [JsonProperty("should_limit_list_of_followers")]
    public bool ShouldLimitListOfFollowers { get; set; }

    [JsonProperty("use_clickable_see_more")]
    public bool UseClickableSeeMore { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }
}
