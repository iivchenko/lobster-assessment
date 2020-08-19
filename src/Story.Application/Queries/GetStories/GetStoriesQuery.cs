using MediatR;

namespace Story.Application.Queries.GetStories
{
    public sealed class GetStoriesQuery : IRequest<GetStoriesQueryResponse>
    {
    }
}
