#region сборка FM.LiveSwitch, Version=1.11.3.40505, Culture=neutral, PublicKeyToken=null
// C:\Unity\Projects\LibTest\LibTest\Assets\Resources\Not separate dlls\FM.LiveSwitch.dll
// Decompiled with ICSharpCode.Decompiler 4.0.0.4521
#endregion

using System;
using System.Collections.Generic;

namespace FM.LiveSwitch
{
    public class CustomLocalMedia : CustomLocalMediaBase<CustomLocalMedia, AudioTrack, VideoTrack>, ICustomLocalMedia<CustomLocalMedia, AudioTrack, VideoTrack>, IMedia<AudioTrack, VideoTrack>
    {
        public SourceInput AudioSourceInput
        {
            get
            {
                return base.AudioTrack?.SourceInput;
            }
            set
            {
                AudioTrack audioTrack = base.AudioTrack;
                if (audioTrack != null)
                {
                    audioTrack.SourceInput = value;
                }
            }
        }

        public SourceInput VideoSourceInput
        {
            get
            {
                return base.VideoTrack?.SourceInput;
            }
            set
            {
                VideoTrack videoTrack = base.VideoTrack;
                if (videoTrack != null)
                {
                    videoTrack.SourceInput = value;
                }
            }
        }

        public AudioSource AudioSource
        {
            get
            {
                AudioSource[] audioSources = AudioSources;
                if (audioSources == null || audioSources.Length == 0)
                {
                    return null;
                }
                return audioSources[0];
            }
        }

        public AudioSource[] AudioSources
        {
            get
            {
                List<AudioSource> list = new List<AudioSource>();
                AudioTrack[] audioTracks = AudioTracks;
                foreach (AudioTrack audioTrack in audioTracks)
                {
                    if (audioTrack.Source != null)
                    {
                        list.Add(audioTrack.Source);
                    }
                }
                return list.ToArray();
            }
        }

        public VideoSource VideoSource
        {
            get
            {
                VideoSource[] videoSources = VideoSources;
                if (videoSources == null || videoSources.Length == 0)
                {
                    return null;
                }
                return videoSources[0];
            }
        }

        public VideoSource[] VideoSources
        {
            get
            {
                List<VideoSource> list = new List<VideoSource>();
                VideoTrack[] videoTracks = VideoTracks;
                foreach (VideoTrack videoTrack in videoTracks)
                {
                    if (videoTrack.Source != null)
                    {
                        list.Add(videoTrack.Source);
                    }
                }
                return list.ToArray();
            }
        }

        public MediaSourceBase[] MediaSources
        {
            get
            {
                List<MediaSourceBase> list = new List<MediaSourceBase>();
                list.AddRange(AudioSources);
                list.AddRange(VideoSources);
                return list.ToArray();
            }
        }

        public event Action0 OnAudioStarted;

        public event Action0 OnVideoStarted;

        public event Action0 OnAudioStopped;

        public event Action0 OnVideoStopped;

        protected override void AddAudioTrack(AudioTrack audioTrack)
        {
            if (audioTrack != null && AudioTracks.Length == 0)
            {
                audioTrack.OnStarted += AudioTrack_OnStarted;
                audioTrack.OnStopped += AudioTrack_OnStopped;
            }
            base.AddAudioTrack(audioTrack);
        }

        protected override bool RemoveAudioTrack(AudioTrack audioTrack)
        {
            if (audioTrack != null)
            {
                audioTrack.OnStarted -= AudioTrack_OnStarted;
                audioTrack.OnStopped -= AudioTrack_OnStopped;
            }
            return base.RemoveAudioTrack(audioTrack);
        }

        private void AudioTrack_OnStarted()
        {
            this.OnAudioStarted?.Invoke();
        }

        private void AudioTrack_OnStopped()
        {
            this.OnAudioStopped?.Invoke();
        }

        protected override void AddVideoTrack(VideoTrack videoTrack)
        {
            if (videoTrack != null && VideoTracks.Length == 0)
            {
                videoTrack.OnStarted += VideoTrack_OnStarted;
                videoTrack.OnStopped += VideoTrack_OnStopped;
            }
            base.AddVideoTrack(videoTrack);
        }

        protected override bool RemoveVideoTrack(VideoTrack videoTrack)
        {
            if (videoTrack != null)
            {
                videoTrack.OnStarted -= VideoTrack_OnStarted;
                videoTrack.OnStopped -= VideoTrack_OnStopped;
            }
            return base.RemoveVideoTrack(videoTrack);
        }

        private void VideoTrack_OnStarted()
        {
            this.OnVideoStarted?.Invoke();
        }

        private void VideoTrack_OnStopped()
        {
            this.OnVideoStopped?.Invoke();
        }

        public Future<SourceInput[]> GetAudioSourceInputs()
        {
            AudioTrack audioTrack = base.AudioTrack;
            if (audioTrack != null)
            {
                return audioTrack.GetSourceInputs();
            }
            Promise<SourceInput[]> promise = new Promise<SourceInput[]>();
            promise.Resolve(new SourceInput[0]);
            return promise;
        }

        public Future<SourceInput[]> GetVideoSourceInputs()
        {
            VideoTrack videoTrack = base.VideoTrack;
            if (videoTrack != null)
            {
                return videoTrack.GetSourceInputs();
            }
            Promise<SourceInput[]> promise = new Promise<SourceInput[]>();
            promise.Resolve(new SourceInput[0]);
            return promise;
        }

        public Future<object> ChangeAudioSourceInput(SourceInput audioSourceInput)
        {
            AudioTrack audioTrack = base.AudioTrack;
            if (audioTrack != null)
            {
                return audioTrack.ChangeSourceInput(audioSourceInput);
            }
            Promise<object> promise = new Promise<object>();
            promise.Reject(new Exception("No audio track."));
            return promise;
        }

        public Future<object> ChangeVideoSourceInput(SourceInput videoSourceInput)
        {
            VideoTrack videoTrack = base.VideoTrack;
            if (videoTrack != null)
            {
                return videoTrack.ChangeSourceInput(videoSourceInput);
            }
            Promise<object> promise = new Promise<object>();
            promise.Reject(new Exception("No video track."));
            return promise;
        }

        protected override CustomAudioEncodingConfig[] DoGetAudioEncodings()
        {
            throw new Exception("Implement DoGetAudioEncodings in your LocalMedia subclass.");
        }

        protected override void DoSetAudioEncodings(CustomAudioEncodingConfig[] encodings)
        {
            throw new Exception("Implement DoSetAudioEncodings in your LocalMedia subclass.");
        }

        protected override CustomVideoEncodingConfig[] DoGetVideoEncodings()
        {
            throw new Exception("Implement DoGetVideoEncodings in your LocalMedia subclass.");
        }

        protected override void DoSetVideoEncodings(CustomVideoEncodingConfig[] encodings)
        {
            throw new Exception("Implement DoSetVideoEncodings in your LocalMedia subclass.");
        }

        protected override Future<CustomLocalMedia> DoStart()
        {
            Promise<CustomLocalMedia> promise = new Promise<CustomLocalMedia>();
            MediaSourceBase[] mediaSources = MediaSources;
            if (mediaSources.Length != 0)
            {
                DoStartSource(promise, mediaSources, 0);
            }
            else
            {
                promise.Resolve(this);
            }
            return promise;
        }

        private void DoStartSource(Promise<CustomLocalMedia> promise, MediaSourceBase[] mediaSources, int index)
        {
            if (index < mediaSources.Length)
            {
                mediaSources[index].Start().Then(delegate
                {
                    DoStartSource(promise, mediaSources, index + 1);
                }, delegate (Exception ex)
                {
                    AbortStart(promise, ex);
                });
            }
            else
            {
                promise.Resolve(this);
            }
        }

        protected override Future<CustomLocalMedia> DoStop()
        {
            Promise<CustomLocalMedia> promise = new Promise<CustomLocalMedia>();
            MediaSourceBase[] mediaSources = MediaSources;
            if (mediaSources.Length != 0)
            {
                DoStopSource(promise, mediaSources, 0);
            }
            else
            {
                promise.Resolve(this);
            }
            return promise;
        }

        private void DoStopSource(Promise<CustomLocalMedia> promise, MediaSourceBase[] mediaSources, int index)
        {
            if (index < mediaSources.Length)
            {
                mediaSources[index].Stop().Then(delegate
                {
                    DoStopSource(promise, mediaSources, index + 1);
                }, delegate (Exception ex)
                {
                    promise.Reject(ex);
                });
            }
            else
            {
                promise.Resolve(this);
            }
        }

        protected override List<AudioTrack> CreateAudioTrackCollection()
        {
            return new List<AudioTrack>();
        }

        protected override List<VideoTrack> CreateVideoTrackCollection()
        {
            return new List<VideoTrack>();
        }

        protected override AudioTrack[] ArrayFromAudioTracks(List<AudioTrack> tracks)
        {
            return tracks.ToArray();
        }

        protected override VideoTrack[] ArrayFromVideoTracks(List<VideoTrack> tracks)
        {
            return tracks.ToArray();
        }
    }
}
