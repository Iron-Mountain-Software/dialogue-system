using UnityEngine;

namespace IronMountain.DialogueSystem.Responses
{
    public abstract class BasicResponse
    {
        public string Text { get; }
        public Sprite Icon { get; }
        public int Row { get; }
        public int Column { get; }
        public IResponseStyle Style { get; }

        public abstract void Execute();
        
        protected BasicResponse(string text, Sprite sprite, int row, int column, IResponseStyle style)
        {
            Text = text;
            Icon = sprite;
            Row = row;
            Column = column;
            Style = style;
        }
    }
}