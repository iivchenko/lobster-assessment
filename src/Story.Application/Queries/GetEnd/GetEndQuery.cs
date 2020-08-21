using MediatR;
using System;

namespace Story.Application.Queries.GetEnd
{
    public sealed class GetEndQuery : IRequest<GetEndQueryResponse>
    {
        public Guid StoryId { get; set; }

        public Guid EndId { get; set; }
    }
}
