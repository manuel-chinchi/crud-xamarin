using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using crud_xamarin.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin
{
    public class ArticleAdapter : RecyclerView.Adapter
    {
        List<Article> articles;

        public ArticleAdapter(List<Article> articles)
        {
            this.articles = articles;
        }

        public override int ItemCount => articles.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as ArticleViewHolder;
            viewHolder.Name.Text = articles[position].Name;
            viewHolder.Details.Text = articles[position].Details;
            viewHolder.Id.Text = articles[position].Id.ToString(); 
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.article_item, parent, false);
            return new ArticleViewHolder(view);
        }
    }

    public class ArticleViewHolder : RecyclerView.ViewHolder
    {
        public TextView Id { get; private set; }
        public TextView Name { get; private set; }
        public TextView Details { get; private set; }

        public ArticleViewHolder(View itemView):base(itemView)
        {
            Id = itemView.FindViewById<TextView>(Resource.Id.colId);
            Name = itemView.FindViewById<TextView>(Resource.Id.colName);
            Details = itemView.FindViewById<TextView>(Resource.Id.colDetails);
            
            //Details = itemView.FindViewById<TextView>(Resource.Id.colDescription);
            //Id = itemView.FindViewById<TextView>(Resource.Id.column3);
            //Name = itemView.FindViewById<TextView>(Resource.Id.column1);
            //Details = itemView.FindViewById<TextView>(Resource.Id.column2);
        }
    }
}