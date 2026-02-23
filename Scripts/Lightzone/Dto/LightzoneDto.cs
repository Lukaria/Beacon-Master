namespace Lightzone.Dto
{
    //todo rework to dictionary
    public struct LightzoneDto
    {
        public float Brightness { get; set; }
        public float Radius { get; set; }
        public float MinAngle { get; set; }
        public float MaxAngle { get; set; }
        public float Speed { get; set; }
    }
}