namespace Story.Application.Domain.Stories.Abstractions
{
    public interface IVisitor
    {
        NodeLeaf VisitQuestion(Question question);

        NodeLeaf VisitAnswer(Answer answer);

        NodeLeaf VisitTheEnd(TheEnd theEnd);
    }
}
