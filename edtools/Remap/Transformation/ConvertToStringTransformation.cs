namespace edtools.Remap.Transformation {
    public class ConvertToStringTransformation : BaseTransformation {

        public override object TransformSingleObject(object inputObject) {
            return CheckType<string>(inputObject);
        }

    }
}
