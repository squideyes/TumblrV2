//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace TumblrV2.Models
//{
//    class Class1
//    {
//        public class Rootobject
//        {
//            public Meta meta { get; set; }
//            public Response response { get; set; }
//        }

//        public class Meta
//        {
//            public int status { get; set; }
//            public string msg { get; set; }
//        }

//        public class Response
//        {
//            public Blog blog { get; set; }
//            public Post[] posts { get; set; }
//            public int total_posts { get; set; }
//        }

//        public class Blog
//        {
//            public bool ask { get; set; }
//            public bool ask_anon { get; set; }
//            public string ask_page_title { get; set; }
//            public bool can_subscribe { get; set; }
//            public string description { get; set; }
//            public bool is_nsfw { get; set; }
//            public string name { get; set; }
//            public int posts { get; set; }
//            public bool share_likes { get; set; }
//            public bool subscribed { get; set; }
//            public string title { get; set; }
//            public int total_posts { get; set; }
//            public int updated { get; set; }
//            public string url { get; set; }
//            public bool is_optout_ads { get; set; }
//        }

//        public class Post
//        {
//            public string type { get; set; }
//            public string blog_name { get; set; }
//            public long id { get; set; }
//            public string post_url { get; set; }
//            public string slug { get; set; }
//            public string date { get; set; }
//            public int timestamp { get; set; }
//            public string state { get; set; }
//            public string format { get; set; }
//            public string reblog_key { get; set; }
//            public string[] tags { get; set; }
//            public string short_url { get; set; }
//            public string summary { get; set; }
//            public bool is_blocks_post_format { get; set; }
//            public object recommended_source { get; set; }
//            public object recommended_color { get; set; }
//            public string post_author { get; set; }
//            public int note_count { get; set; }
//            public string caption { get; set; }
//            public Reblog reblog { get; set; }
//            public Trail[] trail { get; set; }
//            public string video_url { get; set; }
//            public bool html5_capable { get; set; }
//            public string thumbnail_url { get; set; }
//            public int thumbnail_width { get; set; }
//            public int thumbnail_height { get; set; }
//            public float duration { get; set; }
//            public Player[] player { get; set; }
//            public string video_type { get; set; }
//            public bool can_like { get; set; }
//            public bool can_reblog { get; set; }
//            public bool can_send_in_message { get; set; }
//            public bool can_reply { get; set; }
//            public bool display_avatar { get; set; }
//            public string source_url { get; set; }
//            public string source_title { get; set; }
//            public string permalink_url { get; set; }
//        }

//        public class Reblog
//        {
//            public string comment { get; set; }
//            public string tree_html { get; set; }
//        }

//        public class Trail
//        {
//            public Blog1 blog { get; set; }
//            public Post1 post { get; set; }
//            public string content_raw { get; set; }
//            public string content { get; set; }
//            public bool is_root_item { get; set; }
//        }

//        public class Blog1
//        {
//            public string name { get; set; }
//            public bool active { get; set; }
//            public object theme { get; set; }
//            public bool share_likes { get; set; }
//            public bool share_following { get; set; }
//            public bool can_be_followed { get; set; }
//        }

//        public class Post1
//        {
//            public string id { get; set; }
//        }

//        public class Player
//        {
//            public int width { get; set; }
//            public object embed_code { get; set; }
//        }

//    }
//}
