using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Structures
{
    public enum DataType { None = -1, Count = 0, Color = 1, Fill = 2, Shape = 3 }
    public enum Count { One = 0, Two = 1, Three = 2 }
    public enum Colors { Color1 = 0, Color2 = 1, Color3 = 2 }
    public enum Fill { Empty = 0, Grid = 1, Full = 2 }
    public enum Shape { Square = 0, Rounded = 1, Mixed = 2 }
    public static class EnumHelpers
    {
        public static int Count<T>() where T : Enum => ValuesArray<T>().Length;
        public static IEnumerable<T> Values<T>() where T : Enum => ValuesArray<T>().Cast<T>();
        public static Array ValuesArray<T>() where T : Enum => Enum.GetValues(typeof(T));
    }
    public static class Bindings
    {
        public static DataType ToDataType(Type t)
        {
            var sampleData = new CardData();
            var sampleResult = new Result();
            if (t == sampleData.count.GetType() || t == sampleResult.nbOfShape.GetType())
            {
                return DataType.Count;
            }
            else if (t == sampleData.color.GetType() || t == sampleResult.colorOfShapes.GetType())
            {
                return DataType.Color;
            }
            if (t == sampleData.fill.GetType() || t == sampleResult.materialOfShapes.GetType())
            {
                return DataType.Fill;
            }
            if (t == sampleData.shape.GetType() || t == sampleResult.shapeOfShapes.GetType())
            {
                return DataType.Shape;
            }
            return DataType.None;
        }
        public static (Type e, Type t) ToUsedTypes(DataType t)
        {
            var sampleData = new CardData();
            var sampleResult = new Result();
            switch (t)
            {
                case DataType.Count:
                    return (sampleData.count.GetType(), sampleResult.nbOfShape.GetType());
                case DataType.Color:
                    return (sampleData.color.GetType(), sampleResult.colorOfShapes.GetType());
                case DataType.Fill:
                    return (sampleData.fill.GetType(), sampleResult.materialOfShapes.GetType());
                case DataType.Shape:
                    return (sampleData.shape.GetType(), sampleResult.shapeOfShapes.GetType());
                default:
                    return default;
            }
        }
    }
    [Serializable]
    public class Wrapper<T>
    {
        public T Value;
    }
    public struct Result
    {
        public int nbOfShape;
        public Color colorOfShapes;
        public Material materialOfShapes;
        public Sprite shapeOfShapes;

        public Result(int item1, Color item2, Material item3, Sprite item4)
        {
            nbOfShape = item1;
            colorOfShapes = item2;
            materialOfShapes = item3;
            shapeOfShapes = item4;
        }

        public override bool Equals(object obj)
        {
            return obj is Result other &&
                   nbOfShape == other.nbOfShape &&
                   colorOfShapes.Equals(other.colorOfShapes) &&
                   EqualityComparer<Material>.Default.Equals(materialOfShapes, other.materialOfShapes) &&
                   EqualityComparer<Sprite>.Default.Equals(shapeOfShapes, other.shapeOfShapes);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(nbOfShape, colorOfShapes, materialOfShapes, shapeOfShapes);
        }

        public void Deconstruct(out int item1, out Color item2, out Material item3, out Sprite item4)
        {
            item1 = nbOfShape;
            item2 = colorOfShapes;
            item3 = materialOfShapes;
            item4 = shapeOfShapes;
        }

        public static implicit operator (int, Color, Material, Sprite)(Result value)
        {
            return (value.nbOfShape, value.colorOfShapes, value.materialOfShapes, value.shapeOfShapes);
        }

        public static implicit operator Result((int, Color, Material, Sprite) value)
        {
            return new Result(value.Item1, value.Item2, value.Item3, value.Item4);
        }
    }

    [Serializable]
    public struct CardData
    {
        public Count count;
        public int Count => (int)count;
        public Colors color;
        public int Color => (int)color;
        public Fill fill;
        public int Fill => (int)fill;
        public Shape shape;
        public int Shape => (int)shape;

        public CardData(Count item1, Colors item2, Fill item3, Shape item4)
        {
            count = item1;
            color = item2;
            fill = item3;
            shape = item4;
        }

        public override bool Equals(object obj)
        {
            return obj is CardData other &&
                   count == other.count &&
                   color == other.color &&
                   fill == other.fill &&
                   shape == other.shape;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(count, color, fill, shape);
        }

        public void Deconstruct(out Count item1, out Colors item2, out Fill item3, out Shape item4)
        {
            item1 = count;
            item2 = color;
            item3 = fill;
            item4 = shape;
        }

        public static implicit operator (Count, Colors, Fill, Shape)(CardData value)
        {
            return (value.count, value.color, value.fill, value.shape);
        }

        public static implicit operator CardData((Count, Colors, Fill, Shape) value)
        {
            return new CardData(value.Item1, value.Item2, value.Item3, value.Item4);
        }
    }
}

