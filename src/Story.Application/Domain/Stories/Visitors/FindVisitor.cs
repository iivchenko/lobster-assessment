using Story.Application.Domain.Stories.Abstractions;
using System;

namespace Story.Application.Domain.Stories.Visitors
{
    public sealed class FindVisitor : IVisitor
    {
        private readonly Func<NodeLeaf, bool> _predicate;

        public FindVisitor(Func<NodeLeaf, bool> predicate)
        {
            _predicate = predicate;
        }

        public NodeLeaf VisitAnswer(Answer answer)
        {
            throw new NotImplementedException();
        }

        public NodeLeaf VisitQuestion(Question question)
        {
            throw new NotImplementedException();
        }

        public NodeLeaf VisitTheEnd(TheEnd theEnd)
        {
            throw new NotImplementedException();
        }
    }
}
