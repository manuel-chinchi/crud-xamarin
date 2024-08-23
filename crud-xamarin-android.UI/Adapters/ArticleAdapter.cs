using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using crud_xamarin_android.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.UI.Adapters
{
    public class ArticleAdapter : RecyclerView.Adapter
    {
        List<Article> articles;
        List<int> selectedPositions;

        public ArticleAdapter(List<Article> articles)
        {
            this.articles = articles;
            this.selectedPositions = new List<int>();
        }

        public override int ItemCount => articles.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as ArticleViewHolder;
            viewHolder.Name.Text = articles[position].Name;
            viewHolder.Details.Text = articles[position].Details;
            viewHolder.Id.Text = articles[position].Id.ToString();

            viewHolder.Selected.CheckedChange -= null;
            viewHolder.Selected.CheckedChange += (s, e) =>
            {
                if (e.IsChecked)
                {
                    if (!selectedPositions.Contains(holder.Position))
                        selectedPositions.Add(holder.Position);
                }
                else
                {
                    if (selectedPositions.Contains(holder.Position))
                        selectedPositions.Remove(holder.Position);
                }
                ((ArticleActivity)holder.ItemView.Context).ToggleDeleteButton(selectedPositions.Count > 0);
            };
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_article, parent, false);
            return new ArticleViewHolder(view);
        }

        public List<int> GetSelectedPositions()
        {
            // NOTE 23.08.24:If the list of positions ordered from largest to smallest is not returned, deleting rows will present problems
            selectedPositions.Sort((a, b) => b.CompareTo(a));
            return selectedPositions;
        }

        internal void ClearSelectedPositions()
        {
            selectedPositions.Clear();
        }

        internal void RemoveAt(int position)
        {
            articles.RemoveAt(position);
            NotifyItemRemoved(position);
        }
    }

    public class ArticleViewHolder : RecyclerView.ViewHolder
    {
        public TextView Id { get; private set; }
        public TextView Name { get; private set; }
        public TextView Details { get; private set; }
        public CheckBox Selected { get; private set; }

        public ArticleViewHolder(View itemView):base(itemView)
        {
            Id = itemView.FindViewById<TextView>(Resource.Id.colId);
            Name = itemView.FindViewById<TextView>(Resource.Id.colName);
            Details = itemView.FindViewById<TextView>(Resource.Id.colDetails);
            Selected = ItemView.FindViewById<CheckBox>(Resource.Id.chkSelectedArticle);
        }
    }
}