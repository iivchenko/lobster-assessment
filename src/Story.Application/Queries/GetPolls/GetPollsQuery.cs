using MediatR;

namespace Story.Application.Queries.GetPolls
{
    public sealed class GetPollsQuery : IRequest<GetPollsQueryResponse>
    {
    }
}
