using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class VideoReviewClassMap : SubclassMap<VideoReview>
    {
        public VideoReviewClassMap()
        {
            KeyColumn("Id");
            References(x => x.Video);
        }
    }
}
