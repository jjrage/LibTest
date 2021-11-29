#region сборка FM.LiveSwitch, Version=1.11.3.40505, Culture=neutral, PublicKeyToken=null
// C:\Unity\Projects\LibTest\LibTest\Assets\Resources\Not separate dlls\FM.LiveSwitch.dll
// Decompiled with ICSharpCode.Decompiler 4.0.0.4521
#endregion

using FM.LiveSwitch.G722;
using FM.LiveSwitch.H264;
using FM.LiveSwitch.H265;
using FM.LiveSwitch.Opus;
using FM.LiveSwitch.Pcma;
using FM.LiveSwitch.Pcmu;
using FM.LiveSwitch.Vp8;
using FM.LiveSwitch.Vp9;
using System;
using System.Collections.Generic;

namespace FM.LiveSwitch
{
    public abstract class CustomRtcLocalMedia<TView> : CustomLocalMedia, IViewSinkableMedia<TView, ViewSink<TView>>, IViewableMedia<TView>
    {
        private static ILog _log = Log.GetLogger(typeof(RtcLocalMedia<>));

        private AudioConfig _OpusConfig = new AudioConfig(48000, 2);

        private AudioConfig _G722Config = new AudioConfig(16000, 1);

        private AudioConfig _PcmuConfig = new AudioConfig(8000, 1);

        private AudioConfig _PcmaConfig = new AudioConfig(8000, 1);

        private List<AudioSink> _AudioRecorders = new List<AudioSink>();

        private List<VideoSink> _VideoRecorders = new List<VideoSink>();

        private CustomAudioEncodingConfig[] _AudioEncodings = new CustomAudioEncodingConfig[0];

        private CustomVideoEncodingConfig[] _VideoEncodings = new CustomVideoEncodingConfig[0];

        private VideoDegradationPreference _VideoDegradationPreference = VideoDegradationPreference.Automatic;

        private double _VideoBitsPerPixel = CustomLocalMediaBase<CustomLocalMedia, AudioTrack, VideoTrack>.DefaultVideoBitsPerPixel;

        private object _InitializeLock = new object();

        private bool _Initialized;

        private object AudioRecordingLock = new object();

        private object VideoRecordingLock = new object();

        public bool AudioDisabled
        {
            get;
            private set;
        }

        public bool VideoDisabled
        {
            get;
            private set;
        }

        public AecContext AecContext
        {
            get;
            private set;
        }

        public bool AecDisabled
        {
            get
            {
                if (AecContext != null)
                {
                    return AudioDisabled;
                }
                return true;
            }
        }

        public ViewSink<TView> ViewSink
        {
            get;
            private set;
        }

        public virtual TView View
        {
            get
            {
                if (ViewSink != null)
                {
                    return ViewSink.View;
                }
                return default(TView);
            }
        }

        public IAudioOutput[] AudioOutputs
        {
            get
            {
                if (base.AudioTrack != null)
                {
                    return base.AudioTrack.Outputs;
                }
                return null;
            }
        }

        public IVideoOutput[] VideoOutputs
        {
            get
            {
                if (base.VideoTrack != null)
                {
                    return base.VideoTrack.Outputs;
                }
                return null;
            }
        }

        public bool OpusDisabled
        {
            get;
            private set;
        }

        public bool G722Disabled
        {
            get;
            private set;
        }

        public bool PcmuDisabled
        {
            get;
            private set;
        }

        public bool PcmaDisabled
        {
            get;
            private set;
        }

        public bool Vp8Disabled
        {
            get;
            private set;
        }

        public bool Vp9Disabled
        {
            get;
            private set;
        }

        public bool H264Disabled
        {
            get;
            private set;
        }

        public bool H265Disabled
        {
            get;
            private set;
        }

        public AudioPipe OpusConverter => Utility.FirstOrDefault(OpusConverters);

        public AudioPipe[] OpusConverters
        {
            get;
            private set;
        }

        public AudioEncoder OpusEncoder => Utility.FirstOrDefault(OpusEncoders);

        public AudioEncoder[] OpusEncoders
        {
            get;
            private set;
        }

        public AudioPipe OpusPacketizer => Utility.FirstOrDefault(OpusPacketizers);

        public AudioPipe[] OpusPacketizers
        {
            get;
            private set;
        }

        public AudioPipe G722Converter
        {
            get;
            private set;
        }

        public AudioEncoder G722Encoder
        {
            get;
            private set;
        }

        public AudioPipe G722Packetizer
        {
            get;
            private set;
        }

        public AudioPipe PcmuConverter
        {
            get;
            private set;
        }

        public AudioEncoder PcmuEncoder
        {
            get;
            private set;
        }

        public AudioPipe PcmuPacketizer
        {
            get;
            private set;
        }

        public AudioPipe PcmaConverter
        {
            get;
            private set;
        }

        public AudioEncoder PcmaEncoder
        {
            get;
            private set;
        }

        public AudioPipe PcmaPacketizer
        {
            get;
            private set;
        }

        public VideoPipe Vp8Controller => Utility.FirstOrDefault(Vp8Controllers);

        public VideoPipe[] Vp8Controllers
        {
            get;
            private set;
        }

        public VideoPipe Vp8Converter => Utility.FirstOrDefault(Vp8Converters);

        public VideoPipe[] Vp8Converters
        {
            get;
            private set;
        }

        public VideoEncoder Vp8Encoder => Utility.FirstOrDefault(Vp8Encoders);

        public VideoEncoder[] Vp8Encoders
        {
            get;
            private set;
        }

        public VideoPipe Vp8Packetizer => Utility.FirstOrDefault(Vp8Packetizers);

        public VideoPipe[] Vp8Packetizers
        {
            get;
            private set;
        }

        public VideoPipe H264Controller => Utility.FirstOrDefault(H264Controllers);

        public VideoPipe[] H264Controllers
        {
            get;
            private set;
        }

        public VideoPipe H264Converter => Utility.FirstOrDefault(H264Converters);

        public VideoPipe[] H264Converters => Utility.FirstOrDefault(H264ConvertersArray);

        public VideoPipe[][] H264ConvertersArray
        {
            get;
            private set;
        }

        public VideoEncoder H264Encoder => Utility.FirstOrDefault(H264Encoders);

        public VideoEncoder[] H264Encoders => Utility.FirstOrDefault(H264EncodersArray);

        public VideoEncoder[][] H264EncodersArray
        {
            get;
            private set;
        }

        public VideoPipe H264Packetizer => Utility.FirstOrDefault(H264Packetizers);

        public VideoPipe[] H264Packetizers => Utility.FirstOrDefault(H264PacketizersArray);

        public VideoPipe[][] H264PacketizersArray => Utility.FirstOrDefault(H264PacketizersArrayArray);

        public VideoPipe[][][] H264PacketizersArrayArray
        {
            get;
            private set;
        }

        public VideoPipe Vp9Controller => Utility.FirstOrDefault(Vp9Controllers);

        public VideoPipe[] Vp9Controllers
        {
            get;
            private set;
        }

        public VideoPipe Vp9Converter => Utility.FirstOrDefault(Vp9Converters);

        public VideoPipe[] Vp9Converters
        {
            get;
            private set;
        }

        public VideoEncoder Vp9Encoder => Utility.FirstOrDefault(Vp9Encoders);

        public VideoEncoder[] Vp9Encoders
        {
            get;
            private set;
        }

        public VideoPipe Vp9Packetizer => Utility.FirstOrDefault(Vp9Packetizers);

        public VideoPipe[] Vp9Packetizers
        {
            get;
            private set;
        }

        public VideoPipe H265Controller => Utility.FirstOrDefault(H265Controllers);

        public VideoPipe[] H265Controllers
        {
            get;
            private set;
        }

        public VideoPipe H265Converter => Utility.FirstOrDefault(H265Converters);

        public VideoPipe[] H265Converters
        {
            get;
            private set;
        }

        public VideoEncoder H265Encoder => Utility.FirstOrDefault(H265Encoders);

        public VideoEncoder[] H265Encoders
        {
            get;
            private set;
        }

        public VideoPipe H265Packetizer => Utility.FirstOrDefault(H265Packetizers);

        public VideoPipe[] H265Packetizers
        {
            get;
            private set;
        }

        public AudioPipe ActiveAudioConverter
        {
            get;
            private set;
        }

        public AudioEncoder ActiveAudioEncoder
        {
            get;
            private set;
        }

        public AudioPipe ActiveAudioPacketizer
        {
            get;
            private set;
        }

        public VideoPipe ActiveVideoController
        {
            get;
            private set;
        }

        public VideoPipe ActiveVideoConverter
        {
            get;
            private set;
        }

        public VideoEncoder ActiveVideoEncoder
        {
            get;
            private set;
        }

        public VideoPipe ActiveVideoPacketizer
        {
            get;
            private set;
        }

        public VideoDegradationPreference VideoDegradationPreference
        {
            get
            {
                return _VideoDegradationPreference;
            }
            set
            {
                _VideoDegradationPreference = value;
            }
        }

        public double VideoBitsPerPixel
        {
            get
            {
                return _VideoBitsPerPixel;
            }
            set
            {
                if (value < 0.0)
                {
                    throw new Exception("Video bits-per-pixel must be greater than or equal to zero.");
                }
                _VideoBitsPerPixel = value;
            }
        }

        public bool AutomaticVideoDegradation
        {
            get;
            set;
        }

        public event Action1<AudioPipe> OnActiveAudioConverterChange;

        public event Action1<AudioEncoder> OnActiveAudioEncoderChange;

        public event Action1<AudioPipe> OnActiveAudioPacketizerChange;

        public event Action1<VideoPipe> OnActiveVideoControllerChange;

        public event Action1<VideoPipe> OnActiveVideoConverterChange;

        public event Action1<VideoEncoder> OnActiveVideoEncoderChange;

        public event Action1<VideoPipe> OnActiveVideoPacketizerChange;

        protected override CustomAudioEncodingConfig[] DoGetAudioEncodings()
        {
            return _AudioEncodings;
        }

        protected override void DoSetAudioEncodings(CustomAudioEncodingConfig[] encodings)
        {
            _AudioEncodings = encodings;
            ApplyAudioEncodings(encodings);
        }

        protected override CustomVideoEncodingConfig[] DoGetVideoEncodings()
        {
            return _VideoEncodings;
        }

        protected override void DoSetVideoEncodings(CustomVideoEncodingConfig[] encodings)
        {
            _VideoEncodings = encodings;
            ApplyVideoEncodings(encodings);
        }

        public CustomRtcLocalMedia()
            : this(disableAudio: false, disableVideo: false)
        {
        }

        public CustomRtcLocalMedia(bool disableAudio, bool disableVideo)
            : this(disableAudio, disableVideo, (AecContext)null)
        {
        }

        public CustomRtcLocalMedia(bool disableAudio, bool disableVideo, AecContext aecContext)
        {
            if (aecContext != null && aecContext.Processor != null && aecContext.Processor.State != MediaPipeState.Initialized)
            {
                _log.Warn("Local media received a reference to a destroyed AEC context. AEC will be disabled.");
                aecContext = null;
            }
            AudioDisabled = disableAudio;
            VideoDisabled = disableVideo;
            AecContext = aecContext;
            AutomaticVideoDegradation = false;
        }

        private AudioTrack[] CreateAudioTracks(AudioPipe[] converters, AudioEncoder[] encoders, AudioPipe[] packetizers)
        {
            if (converters == null || converters.Length == 0)
            {
                throw new Exception("Can't create local audio track. No converters.");
            }
            if (encoders == null || encoders.Length == 0)
            {
                throw new Exception("Can't create local audio track. No encoders.");
            }
            if (packetizers == null || packetizers.Length == 0)
            {
                throw new Exception("Can't create local audio track. No packetizers.");
            }
            if (converters.Length != encoders.Length)
            {
                throw new Exception("Can't create local audio track. Converter count must match encoder count.");
            }
            if (encoders.Length != packetizers.Length)
            {
                throw new Exception("Can't create local audio track. Encoder count must match packetizer count.");
            }
            List<AudioTrack> list = new List<AudioTrack>();
            for (int i = 0; i < encoders.Length; i++)
            {
                CreateAudioBranch(converters[i], encoders[i], packetizers[i], list, i == 0);
            }
            return list.ToArray();
        }

        private void CreateAudioBranch(AudioPipe converter, AudioEncoder encoder, AudioPipe packetizer, List<AudioTrack> branches, bool addRecorder)
        {
            AudioTrack audioTrack = new AudioTrack(converter).Next(encoder);
            converter.OnProcessFrame += delegate
            {
                if (ActiveAudioConverter != converter)
                {
                    ActiveAudioConverter = converter;
                    this.OnActiveAudioConverterChange?.Invoke(converter);
                }
            };
            encoder.OnProcessFrame += delegate
            {
                if (ActiveAudioEncoder != encoder)
                {
                    ActiveAudioEncoder = encoder;
                    this.OnActiveAudioEncoderChange?.Invoke(encoder);
                }
            };
            List<AudioTrack> list = new List<AudioTrack>();
            if (addRecorder)
            {
                AudioSink audioRecorder = GetAudioRecorder(encoder.OutputFormat);
                if (audioRecorder != null)
                {
                    list.Add(new AudioTrack(audioRecorder));
                }
            }
            AudioTrack item = new AudioTrack(packetizer);
            packetizer.OnProcessFrame += delegate
            {
                if (ActiveAudioPacketizer != packetizer)
                {
                    ActiveAudioPacketizer = packetizer;
                    this.OnActiveAudioPacketizerChange?.Invoke(packetizer);
                }
            };
            list.Add(item);
            audioTrack.Next(list.ToArray());
            branches.Add(audioTrack);
        }

        private VideoTrack[] CreateVideoTracks(VideoPipe[] controllers, VideoPipe[][] converterss, VideoEncoder[][] encoderss, VideoPipe[][][] packetizersss)
        {
            if (controllers == null || controllers.Length == 0)
            {
                throw new Exception("Can't create local video track. No converters.");
            }
            if (converterss == null || converterss.Length == 0)
            {
                throw new Exception("Can't create local video track. No converters.");
            }
            if (encoderss == null || encoderss.Length == 0)
            {
                throw new Exception("Can't create local video track. No encoders.");
            }
            if (packetizersss == null || packetizersss.Length == 0)
            {
                throw new Exception("Can't create local video track. No packetizers.");
            }
            if (controllers.Length != converterss.Length)
            {
                throw new Exception("Can't create local video track. Controller count must match converter array count.");
            }
            if (controllers.Length != encoderss.Length)
            {
                throw new Exception("Can't create local video track. Controller count must match encoder array count.");
            }
            if (controllers.Length != packetizersss.Length)
            {
                throw new Exception("Can't create local video track. Controller count must match packetizer array collection count.");
            }
            for (int i = 0; i < controllers.Length; i++)
            {
                VideoPipe[] array = converterss[i];
                VideoEncoder[] array2 = encoderss[i];
                VideoPipe[][] array3 = packetizersss[i];
                if (array == null || array.Length == 0)
                {
                    throw new Exception("Can't create local video track. No converters.");
                }
                if (array2 == null || array2.Length == 0)
                {
                    throw new Exception("Can't create local video track. No encoders.");
                }
                if (array3 == null || array3.Length == 0)
                {
                    throw new Exception("Can't create local video track. No packetizers.");
                }
                if (array.Length != array2.Length)
                {
                    throw new Exception("Can't create local video track. Converter count must match encoder count.");
                }
                if (array.Length != array3.Length)
                {
                    throw new Exception("Can't create local video track. Converter count must match packetizer array count.");
                }
                for (int j = 0; j < array.Length; j++)
                {
                    VideoPipe[] array4 = array3[j];
                    if (array4 == null || array4.Length == 0)
                    {
                        throw new Exception("Can't create local video track. No packetizers.");
                    }
                }
            }
            List<VideoTrack> list = new List<VideoTrack>();
            for (int k = 0; k < controllers.Length; k++)
            {
                list.Add(CreateVideoBranch(controllers[k], converterss[k], encoderss[k], packetizersss[k], k == 0));
            }
            return list.ToArray();
        }

        private VideoTrack CreateVideoBranch(VideoPipe controller, VideoPipe[] converters, VideoEncoder[] encoders, VideoPipe[][] packetizerss, bool addRecorder)
        {
            controller.OnProcessFrame += delegate
            {
                if (ActiveVideoController != controller)
                {
                    ActiveVideoController = controller;
                    this.OnActiveVideoControllerChange?.Invoke(controller);
                }
            };
            List<VideoTrack> list = new List<VideoTrack>();
            for (int i = 0; i < converters.Length; i++)
            {
                VideoEncoder encoder = encoders[i];
                VideoPipe videoPipe = converters[i];
                ConstrainVideo(controller as FrameRatePipe, videoPipe as ImageScalePipe, encoder);
                list.Add(CreateVideoBranch(videoPipe, encoder, packetizerss[i], addRecorder && i == 0));
            }
            return new VideoTrack(controller).Next(list.ToArray());
        }

        private VideoTrack CreateVideoBranch(VideoPipe converter, VideoEncoder encoder, VideoPipe[] packetizers, bool addRecorder)
        {
            converter.OnProcessFrame += delegate
            {
                if (ActiveVideoConverter != converter)
                {
                    ActiveVideoConverter = converter;
                    this.OnActiveVideoConverterChange?.Invoke(converter);
                }
            };
            encoder.OnProcessFrame += delegate
            {
                if (ActiveVideoEncoder != encoder)
                {
                    ActiveVideoEncoder = encoder;
                    this.OnActiveVideoEncoderChange?.Invoke(encoder);
                }
            };
            List<VideoTrack> list = new List<VideoTrack>();
            if (addRecorder)
            {
                VideoSink videoRecorder = GetVideoRecorder(encoder.OutputFormat);
                if (videoRecorder != null)
                {
                    list.Add(new VideoTrack(videoRecorder));
                }
            }
            foreach (VideoPipe packetizer in packetizers)
            {
                list.Add(CreateVideoBranch(packetizer));
            }
            return new VideoTrack(converter).Next(encoder).Next(list.ToArray());
        }

        private VideoTrack CreateVideoBranch(VideoPipe packetizer)
        {
            packetizer.OnProcessFrame += delegate
            {
                if (ActiveVideoPacketizer != packetizer)
                {
                    ActiveVideoPacketizer = packetizer;
                    this.OnActiveVideoPacketizerChange?.Invoke(packetizer);
                }
            };
            return new VideoTrack(packetizer);
        }

        private bool ConstrainVideo(FrameRatePipe controller, ImageScalePipe converter, VideoEncoder encoder)
        {
            if (controller == null || converter == null)
            {
                return false;
            }
            encoder.OnBitrateChange += delegate
            {
                if (AutomaticVideoDegradation)
                {
                    VideoSource videoSource = base.VideoSource;
                    if (videoSource != null)
                    {
                        int bitrate = encoder.Bitrate;
                        if (bitrate > 0)
                        {
                            Size size = ConstraintUtility.Max(videoSource.MaxOutputSize, videoSource.TargetOutputSize);
                            if (size != null && size.Width > 0 && size.Height > 0)
                            {
                                double num = ConstraintUtility.Max(controller.MaxOutputFrameRate, controller.TargetOutputFrameRate);
                                if (num == -1.0)
                                {
                                    num = ConstraintUtility.Max(videoSource.MaxOutputFrameRate, videoSource.TargetOutputFrameRate);
                                }
                                if (num == -1.0)
                                {
                                    num = 30.0;
                                }
                                int num2 = size.Width * size.Height;
                                double multiplier = (double)VideoUtility.GetPixelCount(bitrate, num, VideoBitsPerPixel) / (double)num2;
                                CustomVideoEncodingConfig encodingConfig = VideoUtility.GetEncodingConfig(VideoUtility.ProcessDegradationPreference(VideoDegradationPreference, videoSource.VideoType), multiplier, num);
                                if (encodingConfig.FrameRate > 0.0)
                                {
                                    controller.TargetFrameRate = MathAssistant.Min(encodingConfig.FrameRate, num);
                                }
                                if (encodingConfig.Scale > 0.0)
                                {
                                    converter.TargetScale = MathAssistant.Min(encodingConfig.Scale, 1.0);
                                }
                            }
                        }
                    }
                }
            };
            return true;
        }

        public bool Initialize()
        {
            return Initialize(null, null);
        }

        public bool Initialize(RtcAudioTrackConfig audioTrackConfig, RtcVideoTrackConfig videoTrackConfig)
        {
            lock (_InitializeLock)
            {
                if (_Initialized)
                {
                    return false;
                }
                _Initialized = true;
            }
            if (audioTrackConfig != null)
            {
                OpusDisabled = audioTrackConfig.OpusDisabled;
                G722Disabled = audioTrackConfig.G722Disabled;
                PcmuDisabled = audioTrackConfig.PcmuDisabled;
                PcmaDisabled = audioTrackConfig.PcmaDisabled;
            }
            if (videoTrackConfig != null)
            {
                Vp8Disabled = videoTrackConfig.Vp8Disabled;
                H264Disabled = videoTrackConfig.H264Disabled;
                Vp9Disabled = videoTrackConfig.Vp9Disabled;
                H265Disabled = videoTrackConfig.H265Disabled;
            }
            try
            {
                if (!AudioDisabled)
                {
                    AudioSource element = CreateAudioSource(_OpusConfig);
                    LockAudioEncodings();
                    CustomAudioEncodingConfig[] audioEncodings = base.AudioEncodings;
                    for (int i = 0; i < audioEncodings.Length; i++)
                    {
                        audioEncodings[i].SynchronizationSource = Utility.GenerateSynchronizationSource();
                    }
                    AudioTrack audioTrack = new AudioTrack(element);
                    if (!AecDisabled)
                    {
                        audioTrack = audioTrack.Next(CreateSoundConverter(AecContext.Processor.Config)).Next(AecContext.Processor);
                    }
                    List<AudioTrack> list = new List<AudioTrack>();
                    if (!OpusDisabled)
                    {
                        OpusConverters = new AudioPipe[base.AudioEncodings.Length];
                        OpusEncoders = new AudioEncoder[base.AudioEncodings.Length];
                        OpusPacketizers = new AudioPipe[base.AudioEncodings.Length];
                        for (int j = 0; j < base.AudioEncodings.Length; j++)
                        {
                            try
                            {
                                OpusEncoders[j] = CreateOpusEncoder(_OpusConfig);
                            }
                            catch (Exception ex)
                            {
                                _log.Error("Could not create local Opus encoder.", ex);
                            }
                            if (OpusEncoders[j] != null)
                            {
                                OpusConverters[j] = CreateSoundConverter(OpusEncoders[j].InputConfig);
                                OpusPacketizers[j] = CreateOpusPacketizer(OpusEncoders[j].OutputConfig);
                            }
                        }
                        if (OpusEncoders.Length != 0 && OpusEncoders[0] != null)
                        {
                            list.AddRange(CreateAudioTracks(OpusConverters, OpusEncoders, OpusPacketizers));
                        }
                        for (int k = 0; k < base.AudioEncodings.Length; k++)
                        {
                            if (OpusEncoders[k] != null && base.AudioEncodings[k].Bitrate == -1)
                            {
                                base.AudioEncodings[k].Bitrate = OpusEncoders[k].TargetOutputBitrate;
                            }
                        }
                        ApplyAudioEncodings(base.AudioEncodings, OpusConverters, OpusEncoders, OpusPacketizers);
                    }
                    if (!G722Disabled)
                    {
                        try
                        {
                            G722Encoder = CreateG722Encoder(_G722Config);
                        }
                        catch (Exception ex2)
                        {
                            _log.Error("Could not create local G.722 encoder.", ex2);
                        }
                        if (G722Encoder != null)
                        {
                            G722Converter = CreateSoundConverter(G722Encoder.InputConfig);
                            G722Packetizer = CreateG722Packetizer(G722Encoder.OutputConfig);
                            list.AddRange(CreateAudioTracks(new AudioPipe[1]
                            {
                                G722Converter
                            }, new AudioEncoder[1]
                            {
                                G722Encoder
                            }, new AudioPipe[1]
                            {
                                G722Packetizer
                            }));
                        }
                        ApplyAudioEncodings(base.AudioEncodings, new AudioPipe[1]
                        {
                            G722Converter
                        }, new AudioEncoder[1]
                        {
                            G722Encoder
                        }, new AudioPipe[1]
                        {
                            G722Packetizer
                        });
                    }
                    if (!PcmuDisabled)
                    {
                        try
                        {
                            PcmuEncoder = CreatePcmuEncoder(_PcmuConfig);
                        }
                        catch (Exception ex3)
                        {
                            _log.Error("Could not create local PCMU encoder.", ex3);
                        }
                        if (PcmuEncoder != null)
                        {
                            PcmuConverter = CreateSoundConverter(PcmuEncoder.InputConfig);
                            PcmuPacketizer = CreatePcmuPacketizer(PcmuEncoder.OutputConfig);
                            list.AddRange(CreateAudioTracks(new AudioPipe[1]
                            {
                                PcmuConverter
                            }, new AudioEncoder[1]
                            {
                                PcmuEncoder
                            }, new AudioPipe[1]
                            {
                                PcmuPacketizer
                            }));
                        }
                        ApplyAudioEncodings(base.AudioEncodings, new AudioPipe[1]
                        {
                            PcmuConverter
                        }, new AudioEncoder[1]
                        {
                            PcmuEncoder
                        }, new AudioPipe[1]
                        {
                            PcmuPacketizer
                        });
                    }
                    if (!PcmaDisabled)
                    {
                        try
                        {
                            PcmaEncoder = CreatePcmaEncoder(_PcmaConfig);
                        }
                        catch (Exception ex4)
                        {
                            _log.Error("Could not create local PCMA encoder.", ex4);
                        }
                        if (PcmaEncoder != null)
                        {
                            PcmaConverter = CreateSoundConverter(PcmaEncoder.InputConfig);
                            PcmaPacketizer = CreatePcmaPacketizer(PcmaEncoder.OutputConfig);
                            list.AddRange(CreateAudioTracks(new AudioPipe[1]
                            {
                                PcmaConverter
                            }, new AudioEncoder[1]
                            {
                                PcmaEncoder
                            }, new AudioPipe[1]
                            {
                                PcmaPacketizer
                            }));
                        }
                        ApplyAudioEncodings(base.AudioEncodings, new AudioPipe[1]
                        {
                            PcmaConverter
                        }, new AudioEncoder[1]
                        {
                            PcmaEncoder
                        }, new AudioPipe[1]
                        {
                            PcmaPacketizer
                        });
                    }
                    if (list.Count <= 0)
                    {
                        throw new Exception("Could not initialize local media. No audio encoders initialized. Check the logs for more detail.");
                    }
                    audioTrack = audioTrack.Next(list.ToArray());
                    AddAudioTrack(audioTrack);
                }
                if (!VideoDisabled)
                {
                    VideoSource videoSource = CreateVideoSource();
                    if (videoSource == null)
                    {
                        LockVideoEncodings();
                    }
                    else
                    {
                        EncodingInfo maxOutputEncoding = videoSource.MaxOutputEncoding;
                        if (maxOutputEncoding == null)
                        {
                            LockVideoEncodings(videoSource.VideoType);
                        }
                        else
                        {
                            LockVideoEncodings(videoSource.VideoType, maxOutputEncoding.ScaledWidth, maxOutputEncoding.ScaledHeight, maxOutputEncoding.FrameRate);
                        }
                    }
                    CustomVideoEncodingConfig[] videoEncodings = base.VideoEncodings;
                    for (int i = 0; i < videoEncodings.Length; i++)
                    {
                        videoEncodings[i].SynchronizationSource = Utility.GenerateSynchronizationSource();
                    }
                    VideoTrack videoTrack = new VideoTrack(videoSource);
                    List<VideoTrack> list2 = new List<VideoTrack>();
                    VideoTrack videoTrack2 = null;
                    try
                    {
                        ViewSink = CreateViewSink();
                    }
                    catch (Exception ex5)
                    {
                        _log.Error("Could not create local view sink.", ex5);
                    }
                    if (ViewSink != null)
                    {
                        VideoFormat videoFormat = ViewSink.InputFormat;
                        if (videoFormat == null)
                        {
                            videoFormat = ((videoSource == null) ? VideoFormat.I420 : videoSource.OutputFormat);
                        }
                        videoTrack2 = new VideoTrack(CreateImageConverter(videoFormat)).Next(ViewSink);
                    }
                    //if (!Vp8Disabled)
                    //{
                    //    Vp8Controllers = new VideoPipe[base.VideoEncodings.Length];
                    //    Vp8Converters = new VideoPipe[base.VideoEncodings.Length];
                    //    Vp8Encoders = new VideoEncoder[base.VideoEncodings.Length];
                    //    Vp8Packetizers = new VideoPipe[base.VideoEncodings.Length];
                    //    for (int l = 0; l < base.VideoEncodings.Length; l++)
                    //    {
                    //        try
                    //        {
                    //            Vp8Encoders[l] = CreateVp8Encoder();
                    //        }
                    //        catch (Exception ex6)
                    //        {
                    //            _log.Error("Could not create local VP8 encoder.", ex6);
                    //        }
                    //        if (Vp8Encoders[l] != null)
                    //        {
                    //            Vp8Controllers[l] = CreateFrameRateController(videoSource.OutputFormat);
                    //            Vp8Converters[l] = CreateImageConverter(Vp8Encoders[l].InputFormat);
                    //            Vp8Packetizers[l] = CreateVp8Packetizer();
                    //        }
                    //    }
                    //    VideoPipe[][] array = new VideoPipe[base.VideoEncodings.Length][];
                    //    VideoEncoder[][] array2 = new VideoEncoder[base.VideoEncodings.Length][];
                    //    VideoPipe[][][] array3 = new VideoPipe[base.VideoEncodings.Length][][];
                    //    for (int m = 0; m < base.VideoEncodings.Length; m++)
                    //    {
                    //        array[m] = new VideoPipe[1]
                    //        {
                    //            Vp8Converters[m]
                    //        };
                    //        array2[m] = new VideoEncoder[1]
                    //        {
                    //            Vp8Encoders[m]
                    //        };
                    //        array3[m] = new VideoPipe[1][]
                    //        {
                    //            new VideoPipe[1]
                    //            {
                    //                Vp8Packetizers[m]
                    //            }
                    //        };
                    //    }
                    //    if (Vp8Encoders.Length != 0 && Vp8Encoders[0] != null)
                    //    {
                    //        list2.AddRange(CreateVideoTracks(Vp8Controllers, array, array2, array3));
                    //    }
                    //    for (int n = 0; n < base.VideoEncodings.Length; n++)
                    //    {
                    //        if (Vp8Encoders[n] != null)
                    //        {
                    //            if (base.VideoEncodings[n].Bitrate == -1)
                    //            {
                    //                base.VideoEncodings[n].Bitrate = Vp8Encoders[n].TargetOutputBitrate;
                    //            }
                    //            if (base.VideoEncodings[n].FrameRate == -1.0)
                    //            {
                    //                base.VideoEncodings[n].FrameRate = Vp8Encoders[n].TargetOutputFrameRate;
                    //            }
                    //            if (base.VideoEncodings[n].Scale == -1.0)
                    //            {
                    //                base.VideoEncodings[n].Scale = Vp8Encoders[n].TargetOutputScale;
                    //            }
                    //        }
                    //    }
                    //    ApplyVideoEncodings(base.VideoEncodings, Vp8Controllers, array, array2, array3);
                    //}
                    if (!H264Disabled)
                    {
                        H264Controllers = new VideoPipe[base.VideoEncodings.Length];
                        H264ConvertersArray = new VideoPipe[base.VideoEncodings.Length][];
                        H264EncodersArray = new VideoEncoder[base.VideoEncodings.Length][];
                        H264PacketizersArrayArray = new VideoPipe[base.VideoEncodings.Length][][];
                        for (int num = 0; num < base.VideoEncodings.Length; num++)
                        {
                            try
                            {
                                H264EncodersArray[num] = (CreateH264Encoders() ?? new VideoEncoder[1]
                                {
                                    CreateH264Encoder()
                                });
                            }
                            catch (Exception ex7)
                            {
                                _log.Error("Could not create local H.264 encoder.", ex7);
                            }
                            if (H264EncodersArray[num] != null)
                            {
                                int num2 = H264EncodersArray[num].Length;
                                H264Controllers[num] = CreateFrameRateController(videoSource.OutputFormat);
                                H264ConvertersArray[num] = new VideoPipe[num2];
                                H264PacketizersArrayArray[num] = new VideoPipe[num2][];
                                for (int num3 = 0; num3 < num2; num3++)
                                {
                                    if (H264EncodersArray[num][num3] != null)
                                    {
                                        H264ConvertersArray[num][num3] = CreateImageConverter(H264EncodersArray[num][num3].InputFormat);
                                        H264PacketizersArrayArray[num][num3] = (CreateH264Packetizers() ?? new VideoPipe[1]
                                        {
                                            CreateH264Packetizer()
                                        });
                                        VideoFormat outputFormat = H264EncodersArray[num][num3].OutputFormat;
                                        if (outputFormat != null)
                                        {
                                            VideoPipe[] array4 = H264PacketizersArrayArray[num][num3];
                                            foreach (VideoPipe obj in array4)
                                            {
                                                VideoFormat inputFormat = obj.InputFormat;
                                                if (inputFormat != null)
                                                {
                                                    inputFormat.Profile = outputFormat.Profile;
                                                    inputFormat.Level = outputFormat.Level;
                                                }
                                                VideoFormat outputFormat2 = obj.OutputFormat;
                                                if (outputFormat2 != null)
                                                {
                                                    outputFormat2.Profile = outputFormat.Profile;
                                                    outputFormat2.Level = outputFormat.Level;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (H264EncodersArray.Length != 0 && H264EncodersArray[0] != null && H264EncodersArray[0].Length != 0 && H264EncodersArray[0][0] != null)
                        {
                            list2.AddRange(CreateVideoTracks(H264Controllers, H264ConvertersArray, H264EncodersArray, H264PacketizersArrayArray));
                        }
                        for (int num4 = 0; num4 < base.VideoEncodings.Length; num4++)
                        {
                            if (H264EncodersArray[num4] != null)
                            {
                                for (int num5 = 0; num5 < H264EncodersArray[num4].Length; num5++)
                                {
                                    if (H264EncodersArray[num4][num5] != null)
                                    {
                                        if (base.VideoEncodings[num4].Bitrate == -1)
                                        {
                                            base.VideoEncodings[num4].Bitrate = H264EncodersArray[num4][num5].TargetOutputBitrate;
                                        }
                                        if (base.VideoEncodings[num4].FrameRate == -1.0)
                                        {
                                            base.VideoEncodings[num4].FrameRate = H264EncodersArray[num4][num5].TargetOutputFrameRate;
                                        }
                                        if (base.VideoEncodings[num4].Scale == -1.0)
                                        {
                                            base.VideoEncodings[num4].Scale = H264EncodersArray[num4][num5].TargetOutputScale;
                                        }
                                    }
                                }
                            }
                        }
                        ApplyVideoEncodings(base.VideoEncodings, H264Controllers, H264ConvertersArray, H264EncodersArray, H264PacketizersArrayArray);
                    }
                    //if (!Vp9Disabled)
                    //{
                    //    Vp9Controllers = new VideoPipe[base.VideoEncodings.Length];
                    //    Vp9Converters = new VideoPipe[base.VideoEncodings.Length];
                    //    Vp9Encoders = new VideoEncoder[base.VideoEncodings.Length];
                    //    Vp9Packetizers = new VideoPipe[base.VideoEncodings.Length];
                    //    for (int num6 = 0; num6 < base.VideoEncodings.Length; num6++)
                    //    {
                    //        try
                    //        {
                    //            Vp9Encoders[num6] = CreateVp9Encoder();
                    //        }
                    //        catch (Exception ex8)
                    //        {
                    //            _log.Error("Could not create local VP9 encoder.", ex8);
                    //        }
                    //        if (Vp9Encoders[num6] != null)
                    //        {
                    //            Vp9Controllers[num6] = CreateFrameRateController(videoSource.OutputFormat);
                    //            Vp9Converters[num6] = CreateImageConverter(Vp9Encoders[num6].InputFormat);
                    //            Vp9Packetizers[num6] = CreateVp9Packetizer();
                    //        }
                    //    }
                    //    VideoPipe[][] array5 = new VideoPipe[base.VideoEncodings.Length][];
                    //    VideoEncoder[][] array6 = new VideoEncoder[base.VideoEncodings.Length][];
                    //    VideoPipe[][][] array7 = new VideoPipe[base.VideoEncodings.Length][][];
                    //    for (int num7 = 0; num7 < base.VideoEncodings.Length; num7++)
                    //    {
                    //        array5[num7] = new VideoPipe[1]
                    //        {
                    //            Vp9Converters[num7]
                    //        };
                    //        array6[num7] = new VideoEncoder[1]
                    //        {
                    //            Vp9Encoders[num7]
                    //        };
                    //        array7[num7] = new VideoPipe[1][]
                    //        {
                    //            new VideoPipe[1]
                    //            {
                    //                Vp9Packetizers[num7]
                    //            }
                    //        };
                    //    }
                    //    if (Vp9Encoders.Length != 0 && Vp9Encoders[0] != null)
                    //    {
                    //        list2.AddRange(CreateVideoTracks(Vp9Controllers, array5, array6, array7));
                    //    }
                    //    for (int num8 = 0; num8 < base.VideoEncodings.Length; num8++)
                    //    {
                    //        if (Vp9Encoders[num8] != null)
                    //        {
                    //            if (base.VideoEncodings[num8].Bitrate == -1)
                    //            {
                    //                base.VideoEncodings[num8].Bitrate = Vp9Encoders[num8].TargetOutputBitrate;
                    //            }
                    //            if (base.VideoEncodings[num8].FrameRate == -1.0)
                    //            {
                    //                base.VideoEncodings[num8].FrameRate = Vp9Encoders[num8].TargetOutputFrameRate;
                    //            }
                    //            if (base.VideoEncodings[num8].Scale == -1.0)
                    //            {
                    //                base.VideoEncodings[num8].Scale = Vp9Encoders[num8].TargetOutputScale;
                    //            }
                    //        }
                    //    }
                    //    ApplyVideoEncodings(base.VideoEncodings, Vp9Controllers, array5, array6, array7);
                    //}
                    if (!H265Disabled)
                    {
                        H265Controllers = new VideoPipe[base.VideoEncodings.Length];
                        H265Converters = new VideoPipe[base.VideoEncodings.Length];
                        H265Encoders = new VideoEncoder[base.VideoEncodings.Length];
                        H265Packetizers = new VideoPipe[base.VideoEncodings.Length];
                        for (int num9 = 0; num9 < base.VideoEncodings.Length; num9++)
                        {
                            try
                            {
                                H265Encoders[num9] = CreateH265Encoder();
                            }
                            catch (Exception ex9)
                            {
                                _log.Error("Could not create local H.265 encoder.", ex9);
                            }
                            if (H265Encoders[num9] != null)
                            {
                                H265Controllers[num9] = CreateFrameRateController(videoSource.OutputFormat);
                                H265Converters[num9] = CreateImageConverter(H265Encoders[num9].InputFormat);
                                H265Packetizers[num9] = CreateH265Packetizer();
                            }
                        }
                        VideoPipe[][] array8 = new VideoPipe[base.VideoEncodings.Length][];
                        VideoEncoder[][] array9 = new VideoEncoder[base.VideoEncodings.Length][];
                        VideoPipe[][][] array10 = new VideoPipe[base.VideoEncodings.Length][][];
                        for (int num10 = 0; num10 < base.VideoEncodings.Length; num10++)
                        {
                            array8[num10] = new VideoPipe[1]
                            {
                                H265Converters[num10]
                            };
                            array9[num10] = new VideoEncoder[1]
                            {
                                H265Encoders[num10]
                            };
                            array10[num10] = new VideoPipe[1][]
                            {
                                new VideoPipe[1]
                                {
                                    H265Packetizers[num10]
                                }
                            };
                        }
                        if (H265Encoders.Length != 0 && H265Encoders[0] != null)
                        {
                            list2.AddRange(CreateVideoTracks(H265Controllers, array8, array9, array10));
                        }
                        for (int num11 = 0; num11 < base.VideoEncodings.Length; num11++)
                        {
                            if (H265Encoders[num11] != null)
                            {
                                if (base.VideoEncodings[num11].Bitrate == -1)
                                {
                                    base.VideoEncodings[num11].Bitrate = H265Encoders[num11].TargetOutputBitrate;
                                }
                                if (base.VideoEncodings[num11].FrameRate == -1.0)
                                {
                                    base.VideoEncodings[num11].FrameRate = H265Encoders[num11].TargetOutputFrameRate;
                                }
                                if (base.VideoEncodings[num11].Scale == -1.0)
                                {
                                    base.VideoEncodings[num11].Scale = H265Encoders[num11].TargetOutputScale;
                                }
                            }
                        }
                        ApplyVideoEncodings(base.VideoEncodings, H265Controllers, array8, array9, array10);
                    }
                    if (list2.Count <= 0)
                    {
                        throw new Exception("Could not initialize local media. No video encoders initialized. Check the logs for more detail.");
                    }
                    if (videoTrack2 != null)
                    {
                        list2.Insert(0, videoTrack2);
                    }
                    videoTrack = videoTrack.Next(list2.ToArray());
                    AddVideoTrack(videoTrack);
                }
                foreach (AudioSink audioRecorder in _AudioRecorders)
                {
                    AttachAudioRecorderSourceEvents(audioRecorder.Input);
                }
                foreach (VideoSink videoRecorder in _VideoRecorders)
                {
                    AttachVideoRecorderSourceEvents(videoRecorder.Input);
                }
            }
            catch (Exception ex10)
            {
                _log.Error("Error initializing local media.", ex10);
                throw ex10;
            }
            return true;
        }

        private void AttachAudioRecorderSourceEvents(IAudioOutput recorderSource)
        {
            if (recorderSource != null)
            {
                recorderSource.OnDisabledChange += delegate
                {
                    if (base.IsRecordingAudio && !recorderSource.Disabled)
                    {
                        foreach (AudioSink audioRecorder in _AudioRecorders)
                        {
                            audioRecorder.Deactivated = !AudioSourceHasSink(recorderSource, audioRecorder);
                        }
                    }
                };
            }
        }

        private void AttachVideoRecorderSourceEvents(IVideoOutput recorderSource)
        {
            if (recorderSource != null)
            {
                recorderSource.OnDisabledChange += delegate
                {
                    if (base.IsRecordingVideo && !recorderSource.Disabled)
                    {
                        foreach (VideoSink videoRecorder in _VideoRecorders)
                        {
                            videoRecorder.Deactivated = !VideoSourceHasSink(recorderSource, videoRecorder);
                        }
                    }
                };
            }
        }

        private static bool AudioSourceHasSink(IAudioOutput source, IAudioInput sink)
        {
            IAudioInput[] outputs = source.Outputs;
            for (int i = 0; i < outputs.Length; i++)
            {
                if (outputs[i] == sink)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool VideoSourceHasSink(IVideoOutput source, IVideoInput sink)
        {
            IVideoInput[] outputs = source.Outputs;
            for (int i = 0; i < outputs.Length; i++)
            {
                if (outputs[i] == sink)
                {
                    return true;
                }
            }
            return false;
        }

        protected abstract AudioSource CreateAudioSource(AudioConfig config);

        protected abstract VideoSource CreateVideoSource();

        protected abstract ViewSink<TView> CreateViewSink();

        protected virtual AudioEncoder CreateG722Encoder(AudioConfig config)
        {
            return new FM.LiveSwitch.G722.Encoder(config);
        }

        protected virtual AudioPipe CreateG722Packetizer(AudioConfig config)
        {
            return new FM.LiveSwitch.G722.Packetizer(config);
        }

        protected virtual AudioEncoder CreatePcmuEncoder(AudioConfig config)
        {
            return new FM.LiveSwitch.Pcmu.Encoder(config);
        }

        protected virtual AudioPipe CreatePcmuPacketizer(AudioConfig config)
        {
            return new FM.LiveSwitch.Pcmu.Packetizer(config);
        }

        protected virtual AudioEncoder CreatePcmaEncoder(AudioConfig config)
        {
            return new FM.LiveSwitch.Pcma.Encoder(config);
        }

        protected virtual AudioPipe CreatePcmaPacketizer(AudioConfig config)
        {
            return new FM.LiveSwitch.Pcma.Packetizer(config);
        }

        protected abstract AudioEncoder CreateOpusEncoder(AudioConfig config);

        protected virtual AudioPipe CreateOpusPacketizer(AudioConfig config)
        {
            return new FM.LiveSwitch.Opus.Packetizer(config);
        }

        //protected abstract VideoEncoder CreateVp8Encoder();

        //protected virtual VideoPipe CreateVp8Packetizer()
        //{
        //    return new FM.LiveSwitch.Vp8.Packetizer();
        //}

        //protected abstract VideoEncoder CreateVp9Encoder();

        //protected virtual VideoPipe CreateVp9Packetizer()
        //{
        //    return new FM.LiveSwitch.Vp9.Packetizer();
        //}

        protected abstract VideoEncoder CreateH264Encoder();

        protected virtual VideoEncoder[] CreateH264Encoders()
        {
            return null;
        }

        protected virtual VideoPipe CreateH264Packetizer()
        {
            return new FM.LiveSwitch.H264.Packetizer();
        }

        protected virtual VideoPipe[] CreateH264Packetizers()
        {
            return null;
        }

        protected virtual VideoEncoder CreateH265Encoder()
        {
            return null;
        }

        protected virtual VideoPipe CreateH265Packetizer()
        {
            return new FM.LiveSwitch.H265.Packetizer();
        }

        protected virtual AudioPipe CreateSoundConverter(AudioConfig config)
        {
            return new SoundConverter(config);
        }

        protected virtual VideoPipe CreateFrameRateController(VideoFormat outputFormat)
        {
            return new FrameRateController(outputFormat);
        }

        protected abstract VideoPipe CreateImageConverter(VideoFormat outputFormat);

        protected abstract AudioSink CreateAudioRecorder(AudioFormat inputFormat);

        protected abstract VideoSink CreateVideoRecorder(VideoFormat inputFormat);

        private AudioSink GetAudioRecorder(AudioFormat inputFormat)
        {
            AudioSink audioSink = CreateAudioRecorder(inputFormat);
            if (audioSink != null)
            {
                audioSink.Deactivated = true;
                _AudioRecorders.Add(audioSink);
            }
            return audioSink;
        }

        private VideoSink GetVideoRecorder(VideoFormat inputFormat)
        {
            VideoSink videoSink = CreateVideoRecorder(inputFormat);
            if (videoSink != null)
            {
                videoSink.Deactivated = true;
                _VideoRecorders.Add(videoSink);
            }
            return videoSink;
        }

        public override void Destroy()
        {
            if (!AecDisabled)
            {
                AecContext.Destroy();
            }
            foreach (AudioSink audioRecorder in _AudioRecorders)
            {
                if (!audioRecorder.Persistent)
                {
                    audioRecorder.Destroy();
                }
            }
            foreach (VideoSink videoRecorder in _VideoRecorders)
            {
                if (!videoRecorder.Persistent)
                {
                    videoRecorder.Destroy();
                }
            }
            base.Destroy();
        }

        private void ApplyAudioEncodings(CustomAudioEncodingConfig[] encodings)
        {
            ApplyAudioEncodings(encodings, OpusConverters, OpusEncoders, OpusPacketizers);
            ApplyAudioEncodings(encodings, new AudioPipe[1]
            {
                G722Converter
            }, new AudioEncoder[1]
            {
                G722Encoder
            }, new AudioPipe[1]
            {
                G722Packetizer
            });
            ApplyAudioEncodings(encodings, new AudioPipe[1]
            {
                PcmuConverter
            }, new AudioEncoder[1]
            {
                PcmuEncoder
            }, new AudioPipe[1]
            {
                PcmuPacketizer
            });
            ApplyAudioEncodings(encodings, new AudioPipe[1]
            {
                PcmaConverter
            }, new AudioEncoder[1]
            {
                PcmaEncoder
            }, new AudioPipe[1]
            {
                PcmaPacketizer
            });
        }

        private void ApplyVideoEncodings(CustomVideoEncodingConfig[] encodings)
        {
            VideoPipe[][] array = new VideoPipe[encodings.Length][];
            VideoEncoder[][] array2 = new VideoEncoder[encodings.Length][];
            VideoPipe[][][] array3 = new VideoPipe[encodings.Length][][];
            for (int i = 0; i < encodings.Length; i++)
            {
                array[i] = new VideoPipe[1]
                {
                    (Vp8Converters == null) ? null : Vp8Converters[i]
                };
                array2[i] = new VideoEncoder[1]
                {
                    (Vp8Encoders == null) ? null : Vp8Encoders[i]
                };
                array3[i] = new VideoPipe[1][]
                {
                    new VideoPipe[1]
                    {
                        (Vp8Packetizers == null) ? null : Vp8Packetizers[i]
                    }
                };
            }
            VideoPipe[][] array4 = new VideoPipe[encodings.Length][];
            VideoEncoder[][] array5 = new VideoEncoder[encodings.Length][];
            VideoPipe[][][] array6 = new VideoPipe[encodings.Length][][];
            for (int j = 0; j < encodings.Length; j++)
            {
                array4[j] = new VideoPipe[1]
                {
                    (Vp9Converters == null) ? null : Vp9Converters[j]
                };
                array5[j] = new VideoEncoder[1]
                {
                    (Vp9Encoders == null) ? null : Vp9Encoders[j]
                };
                array6[j] = new VideoPipe[1][]
                {
                    new VideoPipe[1]
                    {
                        (Vp9Packetizers == null) ? null : Vp9Packetizers[j]
                    }
                };
            }
            VideoPipe[][] array7 = new VideoPipe[encodings.Length][];
            VideoEncoder[][] array8 = new VideoEncoder[encodings.Length][];
            VideoPipe[][][] array9 = new VideoPipe[encodings.Length][][];
            for (int k = 0; k < encodings.Length; k++)
            {
                array7[k] = new VideoPipe[1]
                {
                    (H265Converters == null) ? null : H265Converters[k]
                };
                array8[k] = new VideoEncoder[1]
                {
                    (H265Encoders == null) ? null : H265Encoders[k]
                };
                array9[k] = new VideoPipe[1][]
                {
                    new VideoPipe[1]
                    {
                        (H265Packetizers == null) ? null : H265Packetizers[k]
                    }
                };
            }
            ApplyVideoEncodings(encodings, Vp8Controllers, array, array2, array3);
            ApplyVideoEncodings(encodings, H264Controllers, H264ConvertersArray, H264EncodersArray, H264PacketizersArrayArray);
            ApplyVideoEncodings(encodings, Vp9Controllers, array4, array5, array6);
            ApplyVideoEncodings(encodings, H265Controllers, array7, array8, array9);
        }

        private void ApplyAudioEncodings(CustomAudioEncodingConfig[] encodings, AudioPipe[] converters, AudioEncoder[] encoders, AudioPipe[] packetizers)
        {
            if (converters != null)
            {
                for (int i = 0; i < converters.Length; i++)
                {
                    AudioPipe audioPipe = converters[i];
                    if (audioPipe != null)
                    {
                        long synchronizationSource = encodings[i].SynchronizationSource;
                        if (synchronizationSource >= 0 && audioPipe.OutputSynchronizationSource < 0)
                        {
                            audioPipe.OutputSynchronizationSource = synchronizationSource;
                        }
                    }
                }
            }
            if (encoders != null)
            {
                for (int j = 0; j < encoders.Length; j++)
                {
                    AudioEncoder audioEncoder = encoders[j];
                    if (audioEncoder != null && !audioEncoder.OutputFormat.IsFixedBitrate)
                    {
                        int bitrate = encodings[j].Bitrate;
                        try
                        {
                            if (bitrate > 0)
                            {
                                audioEncoder.MaxBitrate = bitrate;
                                audioEncoder.TargetBitrate = bitrate;
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.Error($"Could not apply {audioEncoder.Label} target bitrate of {bitrate.ToString()}kbps.", ex);
                        }
                    }
                }
            }
            if (packetizers == null)
            {
                return;
            }
            for (int k = 0; k < packetizers.Length; k++)
            {
                AudioPipe audioPipe2 = packetizers[k];
                if (audioPipe2 != null)
                {
                    audioPipe2.Deactivated = encodings[k].Deactivated;
                }
            }
        }

        private void ApplyVideoEncodings(CustomVideoEncodingConfig[] encodings, VideoPipe[] controllers, VideoPipe[][] converterss, VideoEncoder[][] encoderss, VideoPipe[][][] packetizersss)
        {
            if (controllers != null)
            {
                for (int i = 0; i < controllers.Length; i++)
                {
                    FrameRatePipe frameRatePipe = controllers[i] as FrameRatePipe;
                    if (frameRatePipe != null)
                    {
                        long synchronizationSource = encodings[i].SynchronizationSource;
                        if (synchronizationSource >= 0 && frameRatePipe.OutputSynchronizationSource < 0)
                        {
                            frameRatePipe.OutputSynchronizationSource = synchronizationSource;
                        }
                        double frameRate = encodings[i].FrameRate;
                        try
                        {
                            if (frameRate > 0.0)
                            {
                                frameRatePipe.MaxFrameRate = frameRate;
                                frameRatePipe.TargetFrameRate = frameRate;
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.Error($"Could not apply {frameRatePipe.Label} target frame-rate of {frameRate.ToString()}fps.", ex);
                        }
                    }
                    else if (controllers[i] != null)
                    {
                        _log.Warn($"Could not cast {frameRatePipe.Label} to FrameRatePipe. Update this class to inherit from FrameRatePipe to support frame-rate updates.");
                    }
                }
            }
            if (converterss != null)
            {
                for (int j = 0; j < converterss.Length; j++)
                {
                    VideoPipe[] array = converterss[j];
                    if (array == null)
                    {
                        continue;
                    }
                    for (int k = 0; k < array.Length; k++)
                    {
                        ImageScalePipe imageScalePipe = array[k] as ImageScalePipe;
                        if (imageScalePipe != null)
                        {
                            double scale = encodings[j].Scale;
                            try
                            {
                                if (scale > 0.0)
                                {
                                    imageScalePipe.MaxScale = scale;
                                    imageScalePipe.TargetScale = scale;
                                }
                            }
                            catch (Exception ex2)
                            {
                                _log.Error($"Could not apply {imageScalePipe.Label} target scale of {scale.ToString()}.", ex2);
                            }
                        }
                        else if (array[k] != null)
                        {
                            _log.Warn($"Could not cast {imageScalePipe.Label} to ImageScalePipe. Update this class to inherit from ImageScalePipe to support image scale updates.");
                        }
                    }
                }
            }
            if (encoderss != null)
            {
                for (int l = 0; l < encoderss.Length; l++)
                {
                    VideoEncoder[] array2 = encoderss[l];
                    if (array2 == null)
                    {
                        continue;
                    }
                    foreach (VideoEncoder videoEncoder in array2)
                    {
                        if (videoEncoder != null && !videoEncoder.OutputFormat.IsFixedBitrate)
                        {
                            int bitrate = encodings[l].Bitrate;
                            try
                            {
                                if (bitrate > 0)
                                {
                                    videoEncoder.MaxBitrate = bitrate;
                                    videoEncoder.TargetBitrate = bitrate;
                                }
                            }
                            catch (Exception ex3)
                            {
                                _log.Error($"Could not apply {videoEncoder.Label} target bitrate of {bitrate.ToString()}kbps.", ex3);
                            }
                        }
                    }
                }
            }
            if (packetizersss == null)
            {
                return;
            }
            for (int n = 0; n < packetizersss.Length; n++)
            {
                VideoPipe[][] array3 = packetizersss[n];
                if (array3 == null)
                {
                    continue;
                }
                VideoPipe[][] array4 = array3;
                foreach (VideoPipe[] array5 in array4)
                {
                    if (array5 == null)
                    {
                        continue;
                    }
                    foreach (VideoPipe videoPipe in array5)
                    {
                        if (videoPipe != null)
                        {
                            videoPipe.Deactivated = encodings[n].Deactivated;
                        }
                    }
                }
            }
        }

        public bool ToggleAudioRecording()
        {
            lock (AudioRecordingLock)
            {
                base.IsRecordingAudio = !base.IsRecordingAudio;
                if (ViewSink != null)
                {
                    ViewSink.IsRecording = (base.IsRecordingAudio || base.IsRecordingVideo);
                }
                if (base.AudioTrack != null && base.AudioTrack.Outputs.Length != 0 && base.AudioTrack.Outputs[0].Output != null)
                {
                    if (base.IsRecordingAudio)
                    {
                        foreach (AudioSink audioRecorder in _AudioRecorders)
                        {
                            IAudioOutput input = audioRecorder.Input;
                            if (input != null && !input.Disabled)
                            {
                                audioRecorder.Deactivated = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (AudioSink audioRecorder2 in _AudioRecorders)
                        {
                            audioRecorder2.Deactivated = true;
                        }
                    }
                }
                else if (_AudioRecorders.Count > 0)
                {
                    _AudioRecorders[0].Deactivated = !base.IsRecordingAudio;
                }
                return base.IsRecordingAudio;
            }
        }

        public bool ToggleVideoRecording()
        {
            lock (VideoRecordingLock)
            {
                base.IsRecordingVideo = !base.IsRecordingVideo;
                if (ViewSink != null)
                {
                    ViewSink.IsRecording = (base.IsRecordingAudio || base.IsRecordingVideo);
                }
                if (base.VideoTrack != null && base.VideoTrack.Outputs.Length != 0 && base.VideoTrack.Outputs[0].Output != null)
                {
                    if (base.IsRecordingVideo)
                    {
                        foreach (VideoSink videoRecorder in _VideoRecorders)
                        {
                            IVideoOutput input = videoRecorder.Input;
                            if (input != null && !input.Disabled)
                            {
                                videoRecorder.Deactivated = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (VideoSink videoRecorder2 in _VideoRecorders)
                        {
                            videoRecorder2.Deactivated = true;
                        }
                    }
                }
                else if (_VideoRecorders.Count > 0)
                {
                    _VideoRecorders[0].Deactivated = !base.IsRecordingVideo;
                }
                return base.IsRecordingVideo;
            }
        }
    }
}
