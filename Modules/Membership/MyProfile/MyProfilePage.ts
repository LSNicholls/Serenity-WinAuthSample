 import { initFullHeightGridPage } from '@serenity-is/corelib';
import { MyProfileGrid } from './MyProfileGrid';

$(function() {
    initFullHeightGridPage(new MyProfileGrid($('#GridDiv')).element);
});
