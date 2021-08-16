using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace FrontierDevelopments.General.UI
{
    public class TitledSectionComponent : UiComponent
    {
        private const int TitleHeight = 36;
        private const int sectionBorder = 4;
        private const int gapHeight = 12;

        public TitledSectionComponent(string title, List<UiComponent> components)
        {
            Title = title;
            Components = components;
            Height = TitleHeight + components.Aggregate(0,
                (total, component) => total + component.Height + (2 * sectionBorder) + gapHeight);
        }

        private string Title { get; }

        private List<UiComponent> Components { get; }

        public int Height { get; }

        public void Draw(Rect rect)
        {
            var list = new Listing_Standard();
            list.Begin(rect);

            Text.Font = GameFont.Medium;
            list.Label(Title);
            Text.Font = GameFont.Small;

            foreach (var component in Components)
            {
                var section = list.BeginSection(component.Height, sectionBorder, sectionBorder);
                component.Draw(section.GetRect(component.Height));
                list.EndSection(section);
                list.Gap(gapHeight);
            }

            list.End();
        }
    }
}