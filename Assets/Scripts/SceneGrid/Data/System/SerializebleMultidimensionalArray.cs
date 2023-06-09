
[System.Serializable]
public class SerializebleMultidimensionalArray<TElement> 
{
    //  _____________________________________________________________   оюпюлерпш яепхюкхгселнцн екелемрю дбсулепмнцн люяяхбю:

    public int Length_0;
    public int Length_1;

    public TElement Element;


    //  _____________________________________________________________   йнмярпсйрнп яепхюкхгселнцн екелерю дбсулепмнцн люяяхбю:

    public SerializebleMultidimensionalArray(int length_0, int lenth_1, TElement element)
    {
        Length_0 = length_0;
        Length_1 = lenth_1;
        
        Element = element;
    }
}
