// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osuTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Overlays;

namespace osu.Game.Screens.Edit.Components.Timelines.Summary.Parts
{
    public class TimelinePart : TimelinePart<Drawable>
    {
    }

    /// <summary>
    /// Represents a part of the summary timeline..
    /// </summary>
    public class TimelinePart<T> : Container<T> where T : Drawable
    {
        protected readonly IBindable<WorkingBeatmap> Beatmap = new Bindable<WorkingBeatmap>();

        private readonly Container<T> content;

        protected override Container<T> Content => content;

        [Resolved]
        private MusicController musicController { get; set; }

        public TimelinePart(Container<T> content = null)
        {
            AddInternal(this.content = content ?? new Container<T> { RelativeSizeAxes = Axes.Both });

            Beatmap.ValueChanged += b =>
            {
                updateRelativeChildSize();
                LoadBeatmap(b.NewValue);
            };
        }

        [BackgroundDependencyLoader]
        private void load(IBindable<WorkingBeatmap> beatmap)
        {
            Beatmap.BindTo(beatmap);
        }

        private void updateRelativeChildSize()
        {
            // the track may not be loaded completely (only has a length once it is).
            if (!musicController.TrackLoaded)
            {
                content.RelativeChildSize = Vector2.One;
                Schedule(updateRelativeChildSize);
                return;
            }

            Debug.Assert(musicController.CurrentTrack != null);
            content.RelativeChildSize = new Vector2((float)Math.Max(1, musicController.CurrentTrack.Length), 1);
        }

        protected virtual void LoadBeatmap(WorkingBeatmap beatmap)
        {
            content.Clear();
        }
    }
}
