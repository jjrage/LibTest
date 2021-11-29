#region сборка FM.LiveSwitch, Version=1.11.3.40505, Culture=neutral, PublicKeyToken=null
// C:\Unity\Projects\LibTest\LibTest\Assets\Resources\Not separate dlls\FM.LiveSwitch.dll
// Decompiled with ICSharpCode.Decompiler 4.0.0.4521
#endregion

using System;

namespace FM.LiveSwitch
{
    public abstract class CustomLocalMediaBase<ICustomLocalMedia, TAudioTrack, TVideoTrack> : Media<TAudioTrack, TVideoTrack> where ICustomLocalMedia : CustomLocalMediaBase<ICustomLocalMedia, TAudioTrack, TVideoTrack> where TAudioTrack : AudioTrack where TVideoTrack : VideoTrack
    {
        private object _AudioEncodingsSetLock = new object();

        private object _VideoEncodingsSetLock = new object();

        private bool _AudioEncodingsSet;

        private bool _VideoEncodingsSet;

        private CustomAudioSimulcastConfig _AudioSimulcast;

        private CustomVideoSimulcastConfig _VideoSimulcast;

        private object _StateLock = new object();

        private static double _DefaultVideoBitsPerPixel = 0.05;

        public LocalMediaState State
        {
            get;
            private set;
        }

        public CustomAudioEncodingConfig AudioEncoding => Utility.FirstOrDefault(AudioEncodings);

        public CustomAudioEncodingConfig[] AudioEncodings
        {
            get
            {
                return DoGetAudioEncodings();
            }
            set
            {
                if (value == null)
                {
                    value = new CustomAudioEncodingConfig[1]
                    {
                        new CustomAudioEncodingConfig()
                    };
                }
                if (value.Length == 0)
                {
                    throw new Exception("Audio encodings cannot be empty.");
                }
                CustomAudioEncodingConfig[] audioEncodings = AudioEncodings;
                lock (_AudioEncodingsSetLock)
                {
                    if (_AudioEncodingsSet && audioEncodings != null && audioEncodings.Length != value.Length)
                    {
                        throw new Exception("The number of audio encodings cannot be changed once set.");
                    }
                    _AudioEncodingsSet = true;
                }
                DoSetAudioEncodings(value);
            }
        }

        public CustomVideoEncodingConfig VideoEncoding => Utility.FirstOrDefault(VideoEncodings);

        public CustomVideoEncodingConfig[] VideoEncodings
        {
            get
            {
                return DoGetVideoEncodings();
            }
            set
            {
                if (value == null)
                {
                    throw new Exception("Video encodings cannot be null.");
                }
                if (value.Length == 0)
                {
                    throw new Exception("Video encodings cannot be empty.");
                }
                CustomVideoEncodingConfig[] videoEncodings = VideoEncodings;
                lock (_VideoEncodingsSetLock)
                {
                    if (_VideoEncodingsSet && videoEncodings != null && videoEncodings.Length != value.Length)
                    {
                        throw new Exception("The number of video encodings cannot be changed once set.");
                    }
                    _VideoEncodingsSet = true;
                }
                DoSetVideoEncodings(value);
            }
        }

        public bool AudioSimulcastDisabled
        {
            get
            {
                return _AudioSimulcast.Disabled;
            }
            set
            {
                _AudioSimulcast.Disabled = value;
            }
        }

        public int AudioSimulcastEncodingCount
        {
            get
            {
                return _AudioSimulcast.EncodingCount;
            }
            set
            {
                _AudioSimulcast.EncodingCount = value;
            }
        }

        public int AudioSimulcastPreferredBitrate
        {
            get
            {
                return _AudioSimulcast.PreferredBitrate;
            }
            set
            {
                _AudioSimulcast.PreferredBitrate = value;
            }
        }

        public bool VideoSimulcastDisabled
        {
            get
            {
                return _VideoSimulcast.Disabled;
            }
            set
            {
                _VideoSimulcast.Disabled = value;
            }
        }

        public int VideoSimulcastEncodingCount
        {
            get
            {
                return _VideoSimulcast.EncodingCount;
            }
            set
            {
                _VideoSimulcast.EncodingCount = value;
            }
        }

        public int VideoSimulcastPreferredBitrate
        {
            get
            {
                return _VideoSimulcast.PreferredBitrate;
            }
            set
            {
                _VideoSimulcast.PreferredBitrate = value;
            }
        }

        public double VideoSimulcastBitsPerPixel
        {
            get
            {
                return _VideoSimulcast.BitsPerPixel;
            }
            set
            {
                _VideoSimulcast.BitsPerPixel = value;
            }
        }

        public VideoDegradationPreference VideoSimulcastDegradationPreference
        {
            get
            {
                return _VideoSimulcast.DegradationPreference;
            }
            set
            {
                _VideoSimulcast.DegradationPreference = value;
            }
        }

        public static double DefaultVideoBitsPerPixel
        {
            get
            {
                return _DefaultVideoBitsPerPixel;
            }
            set
            {
                if (value < 0.0)
                {
                    throw new Exception("Default bits-per-pixel must be greater than or equal to zero.");
                }
                _DefaultVideoBitsPerPixel = value;
            }
        }

        protected abstract CustomAudioEncodingConfig[] DoGetAudioEncodings();

        protected abstract void DoSetAudioEncodings(CustomAudioEncodingConfig[] encodings);

        protected abstract CustomVideoEncodingConfig[] DoGetVideoEncodings();

        protected abstract void DoSetVideoEncodings(CustomVideoEncodingConfig[] encodings);

        protected void LockAudioEncodings()
        {
            if (AudioEncodings == null || AudioEncodings.Length == 0)
            {
                AudioEncodings = _AudioSimulcast.GetEncodingConfigs();
            }
        }

        protected void LockVideoEncodings()
        {
            LockVideoEncodings(VideoType.Unknown);
        }

        protected void LockVideoEncodings(VideoType sourceType)
        {
            LockVideoEncodings(sourceType, -1, -1, -1.0);
        }

        protected void LockVideoEncodings(VideoType sourceType, int sourceWidth, int sourceHeight, double sourceFrameRate)
        {
            if (VideoEncodings == null || VideoEncodings.Length == 0)
            {
                VideoEncodings = _VideoSimulcast.GetEncodingConfigs(sourceType, sourceWidth, sourceHeight, sourceFrameRate);
            }
        }

        public CustomLocalMediaBase()
        {
            State = LocalMediaState.New;
            InitializeSimulcastConfigs();
        }

        private void InitializeSimulcastConfigs()
        {
            _AudioSimulcast = new CustomAudioSimulcastConfig(1, 32)
            {
                Disabled = true
            };
            _VideoSimulcast = new CustomVideoSimulcastConfig(2, 1024, DefaultVideoBitsPerPixel)
            {
                Disabled = false
            };
        }

        public Future<ICustomLocalMedia> Start()
        {
            return StartInternal(new Promise<ICustomLocalMedia>());
        }

        private Future<ICustomLocalMedia> StartInternal(Promise<ICustomLocalMedia> promise)
        {
            lock (_StateLock)
            {
                switch (State)
                {
                    case LocalMediaState.Starting:
                        promise.Reject(new Exception("Local media cannot be started while it is being started on a different thread."));
                        return promise;
                    case LocalMediaState.Started:
                        promise.Resolve((ICustomLocalMedia)this);
                        return promise;
                    case LocalMediaState.Stopping:
                        promise.Reject(new Exception("Local media cannot be started while it is being stopped."));
                        return promise;
                    case LocalMediaState.Destroying:
                        promise.Reject(new Exception("Local media cannot be started while it is being destroyed."));
                        return promise;
                    case LocalMediaState.Destroyed:
                        promise.Reject(new Exception("Local media cannot be started while it is destroyed."));
                        return promise;
                    default:
                        State = LocalMediaState.Starting;
                        break;
                }
            }
            Log.Debug($"Local media is being started.");
            DoStart().Then(delegate (ICustomLocalMedia result)
            {
                Log.Debug($"Local media has successfully started.");
                lock (_StateLock)
                {
                    State = LocalMediaState.Started;
                    promise.Resolve(result);
                }
            }, delegate (Exception exception)
            {
                Log.Debug($"Local media did not start successfully.");
                lock (_StateLock)
                {
                    State = LocalMediaState.Stopped;
                    promise.Reject(exception);
                }
            });
            return promise;
        }

        protected abstract Future<ICustomLocalMedia> DoStart();

        public Future<ICustomLocalMedia> Stop()
        {
            return StopInternal(new Promise<ICustomLocalMedia>());
        }

        private Future<ICustomLocalMedia> StopInternal(Promise<ICustomLocalMedia> promise)
        {
            lock (_StateLock)
            {
                switch (State)
                {
                    case LocalMediaState.New:
                        promise.Resolve((ICustomLocalMedia)this);
                        return promise;
                    case LocalMediaState.Starting:
                        promise.Reject(new Exception("Local media cannot be stopped while it is being started."));
                        return promise;
                    case LocalMediaState.Stopping:
                        promise.Reject(new Exception("Local media cannot be stopped while it is being stopped on a different thread."));
                        return promise;
                    case LocalMediaState.Stopped:
                        promise.Resolve((ICustomLocalMedia)this);
                        return promise;
                    case LocalMediaState.Destroying:
                        promise.Reject(new Exception("Local media cannot be stopped while it is being destroyed."));
                        return promise;
                    case LocalMediaState.Destroyed:
                        promise.Reject(new Exception("Local media cannot be stopped while it is destroyed."));
                        return promise;
                    default:
                        State = LocalMediaState.Stopping;
                        break;
                }
            }
            Log.Debug($"Local media is being stopped.");
            DoStop().Then(delegate (ICustomLocalMedia result)
            {
                Log.Debug($"Local media has successfully stopped.");
                lock (_StateLock)
                {
                    State = LocalMediaState.Stopped;
                    promise.Resolve(result);
                }
            }, delegate (Exception exception)
            {
                Log.Debug($"Local media did not stop successfully.");
                lock (_StateLock)
                {
                    State = LocalMediaState.Started;
                    promise.Reject(exception);
                }
            });
            return promise;
        }

        protected abstract Future<ICustomLocalMedia> DoStop();

        protected void AbortStart(Promise<ICustomLocalMedia> promise, Exception exception)
        {
            Log.Debug($"Local media failed to start and is in a partial state. Stopping...");
            DoStop().Then(delegate
            {
                Log.Debug($"Local media has successfully stopped.");
                lock (_StateLock)
                {
                    State = LocalMediaState.Stopped;
                    promise.Reject(exception);
                }
            }, delegate
            {
                Log.Debug($"Local media did not stop successfully.");
                lock (_StateLock)
                {
                    State = LocalMediaState.Stopped;
                    promise.Reject(exception);
                }
            });
        }
    }
}
