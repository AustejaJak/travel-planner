<script setup lang="ts">
import Basic_card from "~/components/application-ui/layout/panels/basic_card.vue"
import Social_links_only from "~/components/marketing/sections/footers/social_links_only.vue";
import Dark_with_quick_action from "~/components/application-ui/navigation/navbars/dark_with_quick_action.vue";

</script>

<template>

  <div class="flex flex-col min-h-screen bg-gray-900">
    <Dark_with_quick_action />
    <div class="flex-grow">
      <Basic_card/>
    </div>
    <Social_links_only />
  </div>
</template>
<style scoped>
</style>