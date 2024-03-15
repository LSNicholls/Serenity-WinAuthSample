import { initFullHeightGridPage } from "@serenity-is/corelib"
import { WindowsGroupGrid } from "./WindowsGroupGrid";

export default function pageInit() {
    initFullHeightGridPage(new WindowsGroupGrid($('#GridDiv')).element);
}