using Story.Application.Domain.Stories.Abstractions;

namespace Story.Application.Domain.Stories
{
    public sealed class TheEnd : NodeLeaf
    {
        public string Message { get; set; }

        public override NodeLeaf Accept(IVisitor visitor)
        {
            return visitor.VisitTheEnd(this);
        }
    }
}
