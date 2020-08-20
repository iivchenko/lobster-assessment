using Story.Application.Domain.Stories.Abstractions;

namespace Story.Application.Domain.Stories
{
    public sealed class Question : NodeTree
    {
        public string Text { get; set; }

        public override NodeLeaf Accept(IVisitor visitor)
        {
            return visitor.VisitQuestion(this);
        }
    }
}
