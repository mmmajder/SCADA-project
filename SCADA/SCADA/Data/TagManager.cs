using SCADA.Model.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCADA.Data
{
    public class TagManager
    {
        public static List<AI> AnalogInputTags = new List<AI>();
        public static List<DI> DigitalInputTags = new List<DI>();
        public static List<AO> AnalogOutputTags = new List<AO>();
        public static List<DO> DigitalOutputTags = new List<DO>();
        public static List<InputTag> InputTags = new List<InputTag>();
        public static List<OutputTag> OutputTags = new List<OutputTag>();

        public static void ReadTags()
        {
            EmptyLists();
            XMLManager.LoadTags();
        }

        public static int GenerateTagInvokeId()
        {
            using (var db = new TagInvokedContext())
            {
                return db.InvokedTags.Count();
            }
        }

        public static AI GetAITag(string id)
        {
            foreach (AI tag in AnalogInputTags)
            {
                if (tag.TagName == id)
                {
                    return tag;
                }
            }
            return null;
        }

        public static List<AI> GetAITags()
        {
            return AnalogInputTags;
        }

        public static DI GetDITag(string id)
        {
            foreach (DI tag in DigitalInputTags)
            {
                if (tag.TagName == id)
                {
                    return tag;
                }
            }
            return null;
        }

        public static AO GetAOTag(string id)
        {
            foreach (AO tag in AnalogOutputTags)
            {
                if (tag.TagName == id)
                {
                    return tag;
                }
            }
            return null;
        }

        public static InputTag GetInputTagById(string id)
        {
            foreach (InputTag tag in InputTags)
            {
                if (tag.TagName==id)
                {
                    return tag;
                }
            }
            return null;
        }

        public static OutputTag GetOutputTagById(string id)
        {
            foreach (OutputTag tag in OutputTags)
            {
                if (tag.TagName == id)
                {
                    return tag;
                }
            }
            return null;
        }

        public static List<Tag> GetTags()
        {
            List<Tag> tags = new List<Tag>();
            tags.AddRange(InputTags);
            tags.AddRange(OutputTags);
            return tags;
        }

        public static Tag GetTagById(string id)
        {
            foreach (Tag tag in GetTags())
            {
                if (tag.TagName == id)
                {

                    return tag;
                }
            }
            return null;
        }

        public static TagType GetTagType(string id)
        {
            return GetTagById(id).TagType;
        }

        public static List<InputTag> GetInputTags()
        {
            return InputTags;
        }

        public static void EmptyLists()
        {
            InputTags.Clear();
            OutputTags.Clear();
            AnalogInputTags.Clear();
            AnalogOutputTags.Clear();
            DigitalInputTags.Clear();
            DigitalOutputTags.Clear();
        }
    }
}