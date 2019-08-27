Shader "Custom/Portal"
{
    SubShader
    {
        Pass 
        {
            Stencil 
            {
                Ref 1
                Comp Equal
            }
        }
    }
}
