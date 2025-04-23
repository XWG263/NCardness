using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NBC.ActionEditor;
using System;
using System.Linq;

namespace CCx
{
    public class TimelineGraphProcessor
    {
        private float currentTime;
        private SkillAsset data;
        private float endTime;
        private readonly List<IDirectable> groups;

        private bool m_preInitialized;

        private List<IDirectableTimePointerProcessor> timePointers;
        private List<IDirectableTimePointerProcessor> unsortedStartTimePointers;

        private float startTime;

        private Entity currentOwner;

        public TimelineGraphProcessor(SkillAsset data)
        {
            this.data = data;
        }

        public float StartTime
        {
            get => startTime;
            private set => SetStartTime(value);
        }

        public float EndTime
        {
            get => endTime;
            private set => SetEndTime(value);
        }

        /// <summary>
        ///     当前时间
        /// </summary>
        public float CurrentTime
        {
            get => currentTime;
            set => SetCurrentTime(value);
        }

        public IEnumerable<IDirectable> Children => groups ?? null;


        public bool Active { get; private set; }
        public float Length => data.count;
        public float PreviousTime { get; private set; }

        public GameObject Owner { get; set; }


        public void Reset()
        {
            if (!Active) return;
        }


        public void Dispose()
        {
            

        }

        private void SetStartTime(float value)
        {
            startTime = Mathf.Clamp(value, 0, 10);
        }

        private void SetEndTime(float value)
        {
            endTime = Mathf.Clamp(value, 0, 10);
        }

        private void SetCurrentTime(float value)
        {
            currentTime = Mathf.Clamp(value, 0, 10);
        }

        public void Play(Entity owner)
        {
            currentOwner = owner;
            Play(0, data.count, null,owner);
        }
        public void Play()
        {
            Play(0, data.count, null);
        }

        public void Play(float start)
        {
            Play(start, data.count, null);
        }

        public void Play(float start, Action stopCallback)
        {
            Play(start, data.count, stopCallback);
        }

        public void Play(float start, float end, Action stopCallback,Entity owner = null)
        {
            if (Active) return;

            if (start > end) return;

            StartTime = start;
            EndTime = end;

            Active = true;
            StartTime = start;
            EndTime = end;
            PreviousTime = Mathf.Clamp(start, start, end);
            CurrentTime = Mathf.Clamp(start, start, end);
            //onStop = stopCallback;

            Sample(currentTime);
        }


        public void Sample(float time)
        {
            CurrentTime = time;
            if ((currentTime == 0 || Mathf.Approximately(currentTime, Length)) &&
                Mathf.Approximately(PreviousTime, currentTime)) return;

            if (!m_preInitialized && currentTime > 0 && PreviousTime == 0) InitializePreviewPointers(currentOwner);


            if (timePointers != null) InternalSamplePointers(currentTime, PreviousTime);
        }

        private void InternalSamplePointers(float currentTime, float previousTime)
        {
            if (currentTime > previousTime)
                foreach (var t in timePointers)
                    try
                    {
                        t.TriggerForward(currentTime, previousTime);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }


            if (currentTime < previousTime)
                for (var i = timePointers.Count - 1; i >= 0; i--)
                    try
                    {
                        timePointers[i].TriggerBackward(currentTime, previousTime);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }

            if (unsortedStartTimePointers != null)
                foreach (var t in unsortedStartTimePointers)
                    try
                    {
                        t.Update(currentTime, previousTime);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
        }

        /// <summary>
        ///     初始化时间指针
        /// </summary>
        public void InitializePreviewPointers(Entity owner)
        {
            CommandProcessor.Initialize();
            timePointers = new List<IDirectableTimePointerProcessor>();
            unsortedStartTimePointers = new List<IDirectableTimePointerProcessor>();
            foreach (var group in data.groups.AsEnumerable().Reverse())
            {
                if (!group.IsActive) continue;
                foreach (var track in group.Children.AsEnumerable().Reverse())
                {
                    // var tt = new StartTimePointer(track);

                    foreach (var clip in track.Children)
                    {
                        ClipProcessor tt = (ClipProcessor)CommandProcessor.GetProcessor(clip.GetType());
                        tt.directable = clip;
                        if (owner != null)
                            tt.owner = owner;
                        var p3 = new StartTimePointerProcessor(tt);
                        timePointers.Add(p3);

                        unsortedStartTimePointers.Add(p3);
                        timePointers.Add(new EndTimePointerProcessor(tt));
                    }
                }
            }
            m_preInitialized = true;
        }

    }
}

