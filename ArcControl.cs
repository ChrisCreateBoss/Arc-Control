using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ArcControl
{
    public partial class Arc : Control
    {
        private int _MaxValue;
        private int _MinValue;
        private int _Value;
        private int _Stroke;
        private int _OuterStroke;
        private Color _TextColor;
        private Color _OuterStrokeColor;
        private Color _StrokeColor;
        private Color _MarkerColor;
        private bool _ShowOuterArc;
        private bool _ShowText;
        private bool _ShowMarker;
        private string _InnerText;
        public enum Type { ProgressCircle, CircleSlider };
        private Type _ControlType;
        private Font _InnerTextFont;

        #region Removing Unwanted Properties
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = value; }
        }


        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override RightToLeft RightToLeft
        {
            get { return base.RightToLeft; }
            set { base.RightToLeft = value; }
        }


        #endregion

        #region Adding Properties
        [Category("Active Functions"), Description("Defines which function will take the control")]
        public Type ControlType
        {
            get { return _ControlType; }
            set { _ControlType = value;  Invalidate(); }
        }

        [Category("Active Values"), Description("Defines the maximum value for the control.")]
        public int MaximumValue
        {
            get { return _MaxValue; }
            set { _MaxValue = value;  Invalidate(); }
        }

        [Category("Active Values"), Description("Defines the minimum value for the control.")]
        public int MinimumValue
        {
            get { return _MinValue; }
            set { _MinValue = value; Invalidate(); }
        }

        [Category("Active Values"), Description("Defines the maximum value for the control.")]
        public int ProgressValue
        {
            get { return _Value; }
            set
            {
                if(_Value <= _MaxValue && _Value >= _MinValue)
                {
                    _Value = value;
                    Invalidate();
                }
            }
        }

        [Category("Appearance"), Description("Defines the stroke width of the value arc.")]
        public int InnerArcWidth
        {
            get { return _Stroke; }
            set { _Stroke = value; Invalidate(); }
        }

        [Category("Appearance"), Description("Defines the stroke width of the container arc.")]
        public int OuterArcWidth
        {
            get { return _OuterStroke; }
            set { _OuterStroke = value; Invalidate(); }
        }

        [Category("Appearance"), Description("Defines if the container arc will be shown in the control.")]
        public bool ShowOuterArc
        {
            get { return _ShowOuterArc; }
            set { _ShowOuterArc = value;  Invalidate(); }
        }

        [Category("Appearance"), Description("Defines if the value marker will be shown in the control")]
        public bool ShowValueMarker
        {
            get { return _ShowMarker; }
            set { _ShowMarker = value; Invalidate(); }
        }

        [Category("Appearance"), Description("Defines the color of the value arc")]
        public Color InnerArcColor
        {
            get { return _StrokeColor; }
            set { _StrokeColor = value; Invalidate(); }
        }

        [Category("Appearance"), Description("Defines the color of the container arc")]
        public Color OuterArcColor
        {
            get { return _OuterStrokeColor; }
            set { _OuterStrokeColor = value; Invalidate(); }
        }

        [Category("Appearance"), Description("Defines the color of the value marker")]
        public Color MarkerColor
        {
            get { return _MarkerColor; }
            set { _MarkerColor = value; Invalidate(); }
        }

        [Category("Appearance - Text"), Description("Defines the string displayed in the center of the control")]
        public string InnerText
        {
            get { return _InnerText; }
            set { _InnerText = value; Invalidate(); }
        }

        [Category("Appearance - Text"), Description("Defines the font of the inner text displayed in the center of the control")]
        public Font InnerTextFont
        {
            get { return _InnerTextFont; }
            set { _InnerTextFont = value; Invalidate(); }
        }

        [Category("Appearance - Text"), Description("Defines the color of the inner text displayed in the center of the control")]
        public Color InnerTextColor
        {
            get { return _TextColor; }
            set { _TextColor = value; Invalidate(); }
        }

        [Category("Appearance - Text"), Description("Defines if the text will be shown in the control.")]
        public bool ShowInnerText
        {
            get { return _ShowText; }
            set { _ShowText = value; Invalidate(); }
        }
        #endregion
        public Arc()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Size = new Size(200, 200);
            MaximumValue = 100;
            MinimumValue = 0;
            ProgressValue = 50;
            InnerArcWidth = 10;
            InnerArcColor = Color.YellowGreen;
            InnerTextFont = new Font("Arial", 24, FontStyle.Regular);
            ShowInnerText = true;
            InnerTextColor = Color.White;
            InnerText = "50";
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle rec = Rectangle.Inflate(this.ClientRectangle, -10, -10);
            if (_ShowOuterArc)
            {
                Pen outPen = new Pen(_OuterStrokeColor, _OuterStroke);
                float outerSAngle = 270.0f;
                g.DrawArc(outPen, rec, outerSAngle, 360.0f * _MaxValue);
            }
            if(ControlType == Type.CircleSlider)
            {
                if(_ShowMarker)
                {
                    Pen markPen = new Pen(_MarkerColor, _Stroke - 2);
                    float markSAngle = 270.0f;
                    float percent2 = (float)(_Value - _MinValue) / (float)(_MaxValue - _MinValue);
                    g.DrawArc(markPen, rec, markSAngle, 365.0f * percent2);
                }
            }
            Pen pen = new Pen(_StrokeColor, _Stroke);
            float startAngle = 270.0f;
            float percent = (float)(_Value - _MinValue) / (float)(_MaxValue - _MinValue);
            g.DrawArc(pen, rec, startAngle, 360.0f * percent);
            if (_ShowText)
            {
                var x = (Width / 2) - (g.MeasureString(_InnerText, _InnerTextFont).Width / 2) + 2;
                var y = (Height / 2) - (g.MeasureString(_InnerText, _InnerTextFont).Height / 2) + 3;
                g.DrawString(_InnerText, _InnerTextFont, new SolidBrush(_TextColor), new Point((int)x,(int)y));
            }          
            base.OnPaint(pe);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (ControlType == Type.CircleSlider)
                {
                    var dx = e.X - (Width / 2);
                    var dy = e.Y - (Height / 2);
                    var rad = Math.Atan2(dy, dx);
                    var angle = rad * _MaxValue / Math.PI;
                    int angleInt = Convert.ToInt32(angle);
                    if(angleInt <= _MaxValue && angleInt >= _MinValue)
                    {
                        ProgressValue = angleInt;
                    }
                    OnValueChanged(new ValueChangedEventArgs(24));
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
        }

        public delegate void ProgressValueChangedEventHandler(object sender, ValueChangedEventArgs e);

        [Category("Action"), Description("Fires when the progressValue is changed")]
        public event ProgressValueChangedEventHandler ValueChanged;

        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }
    }

    public class ValueChangedEventArgs : EventArgs
    {
        public int NewValue { get; set; }
        public ValueChangedEventArgs(int newValue)
        {
            NewValue = newValue;
        }
    }
}
