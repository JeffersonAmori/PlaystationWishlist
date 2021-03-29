using PlaystationWishlist.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PlaystationWishlistApp.Behaviors
{
    public class FavoriteButtonBehavior : Behavior<Button>
    {
        protected override void OnAttachedTo(Button button)
        {
            //button.Clicked += Bindable_Clicked;
            button.BindingContextChanged += Button_BindingContextChanged;
            base.OnAttachedTo(button);
        }

        protected override void OnDetachingFrom(Button bindable)
        {
            //bindable.Clicked -= Bindable_Clicked;
            bindable.BindingContextChanged -= Button_BindingContextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void Button_BindingContextChanged(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
                return;

            var game = button.BindingContext as PlaystationGame;
            if (game == null)
                return;

            if (game.IsOnUserWishlist)
            {
                button.BackgroundColor = Color.Red;
                button.Text = "-";
            }
            else
            {
                button.BackgroundColor = Color.Green;
                button.Text = "+";
            }
        }

        //private void Bindable_Clicked(object sender, EventArgs e)
        //{
        //    Button button = sender as Button;
        //    if (button == null)
        //        return;

        //    var game = button.BindingContext as PlaystationGame;
        //    if (game == null)
        //        return;

        //    if (game.IsOnUserWishlist)
        //    {
        //        button.BackgroundColor = Color.Red;
        //        button.Text = "-";
        //    }
        //    else
        //    {
        //        button.BackgroundColor = Color.Green;
        //        button.Text = "+";
        //    }
        //}
    }
}
