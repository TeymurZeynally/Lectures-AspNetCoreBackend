namespace Lecture09.ErrorHandling.CatsApi.Api.Posts.Exceptions
{
    public class PostCatsNotFoundException(string message) : Exception(message)
    {
    }
}
