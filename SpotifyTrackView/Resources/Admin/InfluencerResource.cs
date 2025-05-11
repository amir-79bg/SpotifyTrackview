using SpotifyTrackView.Entity;

namespace SpotifyTrackView.Resources.Admin;

public class InfluencerResource
{
    public static object From(Influencer influencer)
    {
        return new
        {
            influencer.Id,
            influencer.FirstName,
            influencer.LastName,
            influencer.Email,
            influencer.CreatedAt,
            influencer.UpdatedAt,
        };
    }
}