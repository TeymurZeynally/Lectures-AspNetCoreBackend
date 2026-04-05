namespace Lecture10.ClientAppIntegration.CatsApi.Api.Posts.Exceptions
{
    public class PostCatsNotFoundException(string message) : Exception(message)
    {
    }
}
