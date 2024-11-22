<script setup lang="ts">
import Simple_card from "~/components/application-ui/forms/sign-in-forms/simple_card.vue";
import Social_links_only from "~/components/marketing/sections/footers/social_links_only.vue";
import Dark_with_quick_action from "~/components/application-ui/navigation/navbars/dark_with_quick_action.vue";
</script>

<template>
  <div class="flex flex-col min-h-screen bg-gray-900">
    <Dark_with_quick_action />
    <div class="flex-grow">
      <Simple_card />
    </div>
    <Social_links_only />
  </div>
</template>

<style scoped>

</style>