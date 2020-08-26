using MediatR;

namespace Questionnaire.Application.Queries.GetPolls
{
    public sealed class GetPollsQuery : IRequest<GetPollsQueryResponse>
    {
    }
}
