<script setup lang="ts">
import Dark_with_quick_action from "~/components/application-ui/navigation/navbars/dark_with_quick_action.vue";
import Social_links_only from "~/components/marketing/sections/footers/social_links_only.vue";
import AdminTripComponent from "~/components/tripComponents/AdminTripComponent.vue";

definePageMeta({
  middleware: 'redirect-if-logged-out',
});

</script>

<template>
  <div class="flex flex-col min-h-screen bg-gray-900">
    <Dark_with_quick_action />
    <div class="flex-grow">
      <AdminTripComponent/>
    </div>
    <Social_links_only />
  </div>
</template>

<style scoped>

</style>