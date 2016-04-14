using System.Collections;

public class CircularBuffer
{
    private float[] list;
    private int end, start, size;

    public CircularBuffer(int _size, float initialValue = 0f)
    {
        size = _size;
        list = new float[size];
        start = 0;
        end = 0;

        for (int i = 0; i < size; i++)
        {
            list[i] = initialValue;
        }
    }

    public void push(float value)
    {
        if ((end + 1) % size == start)
            start = (start + 1) % size;

        list[end] = value;
        end = (end + 1) % size;
    }

    public float pop()
    {
        if (end != start)
        {
            end = (end - 1 + size) % size;
            return list[end];
        }

        return -42;
    }

    public float getAverage()
    {
        float avg = 0;
        for (int i = 0; i < size; i++)
        {
            avg += list[i];
        }

        avg /= size;

        return avg;
    }
}
