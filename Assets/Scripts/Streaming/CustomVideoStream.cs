#region сборка FM.LiveSwitch, Version=1.11.3.40505, Culture=neutral, PublicKeyToken=null
// C:\Unity\Projects\LibTest\LibTest\Assets\Resources\Not separate dlls\FM.LiveSwitch.dll
// Decompiled with ICSharpCode.Decompiler 4.0.0.4521
#endregion

using FM.LiveSwitch.H264;
using FM.LiveSwitch.Sdp;
using FM.LiveSwitch.Sdp.Rtp;
using System;
using System.Collections.Generic;

namespace FM.LiveSwitch
{
    public class CustomVideoStream : MediaStream<IVideoOutput, IVideoOutputCollection, IVideoInput, IVideoInputCollection, IVideoElement, VideoSource, VideoSink, VideoPipe, VideoTrack, VideoBranch, VideoFrame, VideoBuffer, VideoBufferCollection, VideoFormat, VideoFormatCollection>, IVideoStream, IMediaStream, IStream, IVideoInput, IMediaInput<IVideoOutput, IVideoInput, VideoFrame, VideoBuffer, VideoBufferCollection, VideoFormat>, IInput<IVideoOutput, IVideoInput, VideoFrame, VideoBuffer, VideoBufferCollection, VideoFormat>, IElement, IMediaElement, IVideoElement, IVideoOutput, IMediaOutput<IVideoOutput, IVideoInput, VideoFrame, VideoBuffer, VideoBufferCollection, VideoFormat>, IOutput<IVideoOutput, IVideoInput, VideoFrame, VideoBuffer, VideoBufferCollection, VideoFormat>
    {
        private static ILog _log = Log.GetLogger(typeof(CustomVideoStream));

        private volatile bool ReadyToSend;

        private VideoType _VideoType = VideoType.Unknown;

        public bool Vp8Disabled
        {
            get
            {
                return GetCodecDisabled(VideoFormat.Vp8Name);
            }
            set
            {
                SetCodecDisabled(VideoFormat.Vp8Name, value);
            }
        }

        public bool Vp9Disabled
        {
            get
            {
                return GetCodecDisabled(VideoFormat.Vp9Name);
            }
            set
            {
                SetCodecDisabled(VideoFormat.Vp9Name, value);
            }
        }

        public bool H264Disabled
        {
            get
            {
                return GetCodecDisabled(VideoFormat.H264Name);
            }
            set
            {
                SetCodecDisabled(VideoFormat.H264Name, value);
            }
        }

        public bool H265Disabled
        {
            get
            {
                return GetCodecDisabled(VideoFormat.H265Name);
            }
            set
            {
                SetCodecDisabled(VideoFormat.H265Name, value);
            }
        }

        public virtual VideoType VideoType => _VideoType;

        public override bool OverConstrainedInput
        {
            get
            {
                if (!base.OverConstrainedInput && !OverConstrainedInputScale && !OverConstrainedInputFrameRate)
                {
                    return OverConstrainedInputSize;
                }
                return true;
            }
        }

        public override bool OverConstrainedOutput
        {
            get
            {
                if (!base.OverConstrainedOutput && !OverConstrainedOutputScale && !OverConstrainedOutputFrameRate)
                {
                    return OverConstrainedOutputSize;
                }
                return true;
            }
        }

        public bool OverConstrainedScale
        {
            get
            {
                if (!OverConstrainedInputScale)
                {
                    return OverConstrainedOutputScale;
                }
                return true;
            }
        }

        public bool OverConstrainedInputScale => ConstraintUtility.OverConstrained(MinInputScale, MaxInputScale);

        public bool OverConstrainedOutputScale => ConstraintUtility.OverConstrained(MinOutputScale, MaxOutputScale);

        public virtual double MinInputScale => -1.0;

        public virtual double MaxInputScale => -1.0;

        public virtual double MinOutputScale => -1.0;

        public virtual double MaxOutputScale => -1.0;

        public virtual double TargetOutputScale => -1.0;

        public bool OverConstrainedFrameRate
        {
            get
            {
                if (!OverConstrainedInputFrameRate)
                {
                    return OverConstrainedOutputFrameRate;
                }
                return true;
            }
        }

        public bool OverConstrainedInputFrameRate => ConstraintUtility.OverConstrained(MinInputFrameRate, MaxInputFrameRate);

        public bool OverConstrainedOutputFrameRate => ConstraintUtility.OverConstrained(MinOutputFrameRate, MaxOutputFrameRate);

        public virtual double MinInputFrameRate => -1.0;

        public virtual double MaxInputFrameRate => -1.0;

        public virtual double MinOutputFrameRate => -1.0;

        public virtual double MaxOutputFrameRate => -1.0;

        public virtual double TargetOutputFrameRate => -1.0;

        public bool OverConstrainedSize
        {
            get
            {
                if (!OverConstrainedInputSize)
                {
                    return OverConstrainedOutputSize;
                }
                return true;
            }
        }

        public bool OverConstrainedInputSize => ConstraintUtility.OverConstrained(MinInputSize, MaxInputSize);

        public bool OverConstrainedOutputSize => ConstraintUtility.OverConstrained(MinOutputSize, MaxOutputSize);

        public virtual Size MinInputSize => null;

        public virtual Size MaxInputSize => null;

        public virtual Size MinOutputSize => null;

        public virtual Size MaxOutputSize => null;

        public virtual Size TargetOutputSize => null;

        public bool OverConstrainedInputWidth => ConstraintUtility.OverConstrained(MinInputWidth, MaxInputWidth);

        public bool OverConstrainedOutputWidth => ConstraintUtility.OverConstrained(MinOutputWidth, MaxOutputWidth);

        public virtual int MinInputWidth => ConstraintUtility.GetWidth(MinInputSize);

        public virtual int MaxInputWidth => ConstraintUtility.GetWidth(MaxInputSize);

        public virtual int MinOutputWidth => ConstraintUtility.GetWidth(MinOutputSize);

        public virtual int MaxOutputWidth => ConstraintUtility.GetWidth(MaxOutputSize);

        public virtual int TargetOutputWidth => ConstraintUtility.GetWidth(TargetOutputSize);

        public bool OverConstrainedInputHeight => ConstraintUtility.OverConstrained(MinInputHeight, MaxInputHeight);

        public bool OverConstrainedOutputHeight => ConstraintUtility.OverConstrained(MinOutputHeight, MaxOutputHeight);

        public virtual int MinInputHeight => ConstraintUtility.GetHeight(MinInputSize);

        public virtual int MaxInputHeight => ConstraintUtility.GetHeight(MaxInputSize);

        public virtual int MinOutputHeight => ConstraintUtility.GetHeight(MinOutputSize);

        public virtual int MaxOutputHeight => ConstraintUtility.GetHeight(MaxOutputSize);

        public virtual int TargetOutputHeight => ConstraintUtility.GetHeight(TargetOutputSize);

        public override EncodingInfo MinInputEncoding
        {
            get
            {
                EncodingInfo minInputEncoding = base.MinInputEncoding;
                if (minInputEncoding != null)
                {
                    minInputEncoding.Scale = MinInputScale;
                    minInputEncoding.FrameRate = MinInputFrameRate;
                    minInputEncoding.Size = MinInputSize;
                }
                return minInputEncoding;
            }
        }

        public override EncodingInfo MaxInputEncoding
        {
            get
            {
                EncodingInfo maxInputEncoding = base.MaxInputEncoding;
                if (maxInputEncoding != null)
                {
                    maxInputEncoding.Scale = MaxInputScale;
                    maxInputEncoding.FrameRate = MaxInputFrameRate;
                    maxInputEncoding.Size = MaxInputSize;
                }
                return maxInputEncoding;
            }
        }

        public override EncodingInfo MinOutputEncoding
        {
            get
            {
                EncodingInfo minOutputEncoding = base.MinOutputEncoding;
                if (minOutputEncoding != null)
                {
                    minOutputEncoding.Scale = MinOutputScale;
                    minOutputEncoding.FrameRate = MinOutputFrameRate;
                    minOutputEncoding.Size = MinOutputSize;
                }
                return minOutputEncoding;
            }
        }

        public override EncodingInfo MaxOutputEncoding
        {
            get
            {
                EncodingInfo maxOutputEncoding = base.MaxOutputEncoding;
                if (maxOutputEncoding != null)
                {
                    maxOutputEncoding.Scale = MaxOutputScale;
                    maxOutputEncoding.FrameRate = MaxOutputFrameRate;
                    maxOutputEncoding.Size = MaxOutputSize;
                }
                return maxOutputEncoding;
            }
        }

        public override EncodingInfo TargetOutputEncoding
        {
            get
            {
                EncodingInfo targetOutputEncoding = base.TargetOutputEncoding;
                if (targetOutputEncoding != null)
                {
                    targetOutputEncoding.Scale = TargetOutputScale;
                    targetOutputEncoding.FrameRate = TargetOutputFrameRate;
                    targetOutputEncoding.Size = TargetOutputSize;
                }
                return targetOutputEncoding;
            }
        }

        public event Action1<long[]> OnDiscardKeyFrameRequest;

        public CustomVideoStream()
            : this(new IVideoOutput[0])
        {
        }

        public CustomVideoStream(IVideoOutput input)
            : this((input == null) ? null : new IVideoOutput[1]
            {
                input
            })
        {
        }

        public CustomVideoStream(IVideoOutput[] inputs)
            : this(inputs, null)
        {
        }

        public CustomVideoStream(IVideoInput output)
            : this((output == null) ? null : new IVideoInput[1]
            {
                output
            })
        {
        }

        public CustomVideoStream(IVideoInput[] outputs)
            : this(null, outputs)
        {
        }

        public CustomVideoStream(IVideoOutput input, IVideoInput output)
            : this((input == null) ? null : new IVideoOutput[1]
            {
                input
            }, (output == null) ? null : new IVideoInput[1]
            {
                output
            })
        {
        }

        public CustomVideoStream(VideoTrack localTrack)
            : this(localTrack, null)
        {
        }

        public CustomVideoStream(VideoTrack localTrack, VideoTrack remoteTrack)
            : this((localTrack == null) ? null : localTrack.Outputs, (remoteTrack == null) ? null : remoteTrack.Inputs)
        {
            base.LocalTrack = localTrack;
            base.RemoteTrack = remoteTrack;
        }

        public CustomVideoStream(LocalMedia localMedia)
            : this(localMedia, null)
        {
        }

        public CustomVideoStream(RemoteMedia remoteMedia)
            : this(null, remoteMedia)
        {
        }

        public CustomVideoStream(LocalMedia localMedia, RemoteMedia remoteMedia)
            : this((localMedia == null) ? null : localMedia.VideoTrack, (remoteMedia == null) ? null : remoteMedia.VideoTrack)
        {
            base.LocalMedia = localMedia;
            base.RemoteMedia = remoteMedia;
        }

        public CustomVideoStream(IVideoOutput[] inputs, IVideoInput[] outputs)
            : base(StreamType.Video, new JitterConfig())
        {
            if (inputs == null && outputs == null)
            {
                throw new Exception("Cannot initialize video stream if no inputs and no outputs are provided.");
            }
            base.RedFecPolicy = RedFecPolicy.Disabled;
            base.NackPolicy = NackPolicy.Negotiated;
            base.NackPliPolicy = NackPliPolicy.Negotiated;
            base.CcmFirPolicy = CcmFirPolicy.Negotiated;
            base.CcmLrrPolicy = CcmLrrPolicy.Disabled;
            base.CcmTmmbrPolicy = CcmTmmbrPolicy.Disabled;
            base.CcmTmmbnPolicy = CcmTmmbnPolicy.Disabled;
            base.BandwidthAdaptationPolicy = BandwidthAdaptationPolicy.Enabled;
            if (inputs != null)
            {
                AddInputs(inputs);
            }
            if (outputs != null)
            {
                AddOutputs(outputs);
            }
            base.OnDiscardOutboundControlFrame += VideoStream_OnDiscardOutboundControlFrame;
        }

        protected override VideoFormat[] FormatArrayFromList(List<VideoFormat> formatList)
        {
            return formatList.ToArray();
        }

        protected override IVideoInput[] InputArrayFromList(List<IVideoInput> inputList)
        {
            return inputList.ToArray();
        }

        protected override IVideoOutput[] OutputArrayFromList(List<IVideoOutput> outputList)
        {
            return outputList.ToArray();
        }

        protected override VideoFormat CreateFormat(MapAttribute attribute, FormatParametersAttribute formatParametersAttribute)
        {
            VideoFormat videoFormat = CreateFormat(attribute.FormatName, attribute.ClockRate, attribute.FormatParameters, attribute.PayloadType);
            if (videoFormat.IsH264)
            {
                if (formatParametersAttribute != null)
                {
                    string value = null;
                    if (formatParametersAttribute.TryGetFormatSpecificParameter("packetization-mode", out value))
                    {
                        videoFormat.PacketizationMode = value;
                    }
                    else
                    {
                        videoFormat.PacketizationMode = "0";
                    }
                    string value2 = null;
                    if (formatParametersAttribute.TryGetFormatSpecificParameter("profile-level-id", out value2))
                    {
                        if (value2.Length == 6)
                        {
                            videoFormat.Profile = value2.Substring(0, 4);
                            videoFormat.Level = value2.Substring(4, 2);
                        }
                        else
                        {
                            _log.Warn($"Invalid H.264 profile-level-id: {value2}");
                        }
                    }
                    else
                    {
                        videoFormat.Profile = ProfileLevelId.BaselineLevel10.Profile;
                        videoFormat.Level = ProfileLevelId.BaselineLevel10.Level;
                    }
                    string value3 = null;
                    if (formatParametersAttribute.TryGetFormatSpecificParameter("level-asymmetry-allowed", out value3))
                    {
                        videoFormat.LevelIsStrict = (value3 == "0");
                    }
                    else
                    {
                        videoFormat.LevelIsStrict = true;
                    }
                }
                else
                {
                    videoFormat.PacketizationMode = "0";
                    videoFormat.Profile = ProfileLevelId.BaselineLevel10.Profile;
                    videoFormat.Level = ProfileLevelId.BaselineLevel10.Level;
                    videoFormat.LevelIsStrict = true;
                }
            }
            else if (videoFormat.IsH265 && formatParametersAttribute != null)
            {
                string value4 = null;
                if (formatParametersAttribute.TryGetFormatSpecificParameter("profile-id", out value4))
                {
                    videoFormat.Profile = value4;
                }
                string value5 = null;
                if (formatParametersAttribute.TryGetFormatSpecificParameter("level-id", out value5))
                {
                    videoFormat.Level = value5;
                }
                string value6 = null;
                if (formatParametersAttribute.TryGetFormatSpecificParameter("tier-flag", out value6))
                {
                    videoFormat.Tier = value6;
                }
            }
            return videoFormat;
        }

        protected override VideoFormat CreateFormat(string name, int clockRate, string parameters, int payloadType)
        {
            VideoFormat videoFormat = new VideoFormat(name, clockRate);
            //videoFormat.RegisteredPayloadType = payloadType;
            videoFormat.IsPacketized = true;
            IVideoOutput[] inputs = base.Inputs;
            for (int i = 0; i < inputs.Length; i++)
            {
                VideoFormat outputFormat = inputs[i].OutputFormat;
                if (outputFormat.IsEquivalent(videoFormat))
                {
                    videoFormat.IsInjected = outputFormat.IsInjected;
                }
            }
            IVideoInput[] outputs = base.Outputs;
            for (int i = 0; i < outputs.Length; i++)
            {
                VideoFormat inputFormat = outputs[i].InputFormat;
                if (inputFormat.IsEquivalent(videoFormat))
                {
                    videoFormat.IsInjected = inputFormat.IsInjected;
                }
            }
            return videoFormat;
        }

        protected override IVideoOutputCollection CreateOutputCollection(IVideoInput input)
        {
            return new IVideoOutputCollection(input);
        }

        protected override IVideoInputCollection CreateInputCollection(IVideoOutput output)
        {
            return new IVideoInputCollection(output);
        }

        protected override VideoFormatCollection CreateMediaFormatCollection()
        {
            return new VideoFormatCollection();
        }

        protected override VideoFormat CreateRedFormat()
        {
            return new VideoFormat(MediaFormat<VideoFormat>.RedName)
            {
                IsPacketized = true
            };
        }

        protected override VideoFormat CreateUlpFecFormat()
        {
            return new VideoFormat(MediaFormat<VideoFormat>.UlpFecName)
            {
                IsPacketized = true
            };
        }

        public override bool ProcessFrame(VideoFrame frame)
        {
            if (!ReadyToSend)
            {
                ReadyToSend = frame.LastBuffer.IsKeyFrame;
            }
            if (!ReadyToSend)
            {
                RaiseControlFrame(new FirControlFrame(new FirEntry(GetCcmSequenceNumber()))
                {
                    MediaSourceSynchronizationSource = frame.SynchronizationSource
                });
                return false;
            }
            return base.ProcessFrame(frame);
        }

        protected override bool GetInputSourceMuted(IVideoOutput input)
        {
            VideoSource videoSource = input as VideoSource;
            if (videoSource != null)
            {
                return videoSource.OutputMuted;
            }
            VideoPipe videoPipe = input as VideoPipe;
            if (videoPipe != null)
            {
                return GetInputSourceMuted(videoPipe.Inputs);
            }
            return false;
        }

        protected override void SetInputSourceMuted(IVideoOutput input, bool value)
        {
            VideoSource videoSource = input as VideoSource;
            if (videoSource != null)
            {
                videoSource.OutputMuted = value;
                return;
            }
            VideoPipe videoPipe = input as VideoPipe;
            if (videoPipe != null)
            {
                SetInputSourceMuted(videoPipe.Inputs, value);
            }
        }

        protected override bool GetOutputSinkMuted(IVideoInput output)
        {
            VideoSink videoSink = output as VideoSink;
            if (videoSink != null)
            {
                return videoSink.InputMuted;
            }
            VideoPipe videoPipe = output as VideoPipe;
            if (videoPipe != null)
            {
                return GetOutputSinkMuted(videoPipe.Outputs);
            }
            return false;
        }

        protected override void SetOutputSinkMuted(IVideoInput output, bool value)
        {
            VideoSink videoSink = output as VideoSink;
            if (videoSink != null)
            {
                videoSink.InputMuted = value;
                return;
            }
            VideoPipe videoPipe = output as VideoPipe;
            if (videoPipe != null)
            {
                SetOutputSinkMuted(videoPipe.Outputs, value);
            }
        }

        private void VideoStream_OnDiscardOutboundControlFrame(MediaControlFrame controlFrame)
        {
            if (controlFrame is PliControlFrame)
            {
                PliControlFrame pliControlFrame = (PliControlFrame)controlFrame;
                this.OnDiscardKeyFrameRequest?.Invoke(new long[1]
                {
                    pliControlFrame.MediaSourceSynchronizationSource
                });
            }
            else if (controlFrame is FirControlFrame)
            {
                FirControlFrame firControlFrame = (FirControlFrame)controlFrame;
                this.OnDiscardKeyFrameRequest?.Invoke(new long[1]
                {
                    firControlFrame.MediaSourceSynchronizationSource
                });
            }
            else if (controlFrame is LrrControlFrame)
            {
                LrrControlFrame lrrControlFrame = (LrrControlFrame)controlFrame;
                this.OnDiscardKeyFrameRequest?.Invoke(new long[1]
                {
                    lrrControlFrame.MediaSourceSynchronizationSource
                });
            }
        }

        public void RaiseKeyFrameRequest(long[] synchronizationSources)
        {
            PliControlFrame[] array = new PliControlFrame[synchronizationSources.Length];
            for (int i = 0; i < synchronizationSources.Length; i++)
            {
                array[i] = new PliControlFrame
                {
                    MediaSourceSynchronizationSource = synchronizationSources[i]
                };
            }
            MediaControlFrame[] controlFrames = array;
            RaiseControlFrames(controlFrames);
        }
    }
}
